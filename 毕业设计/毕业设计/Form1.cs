using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Collections;

namespace 毕业设计
{
    public partial class FormTempHumi : Form
    {
        UsbDongle Dongle = new UsbDongle();     //USBDongle对象
        UserConDataShow[] DataShowtable = new UserConDataShow[6];//软件界面显示最多6个设备
        Hashtable DeviceTable = new Hashtable(); //存储搜索到设备
        Timer TimerCheckUpdate = new Timer();   //用于检查数据有没有更新
        TimeSpan UpdateSpan = new TimeSpan();
        private const double UpdateSpanLimit_Second = 8;    //8秒没有更新就消失

        XMLsaver XMLDealer = new XMLsaver();    //xml操作对象

        /// <summary>
        /// 将Device和DataShow界面绑定在一起的结构体
        /// </summary>
        struct DeviceDataShowStruct
        {
            public TempHumiDevice Device;
            public UserConDataShow DataShow;

            /// <summary>
            /// 把Device对象和DataShow对象绑一起
            /// </summary>
            /// <param name="Device"></param>
            /// <param name="DataShow"></param>
            public DeviceDataShowStruct(TempHumiDevice Device, UserConDataShow DataShow)
            {
                this.Device = Device;
                this.DataShow = DataShow;   //互相订阅事件
                this.Device.DeviceUpdateDoneEvent += this.DataShow.DataShowUpdate;
                this.DataShow.NameInAppChangeEvent += this.Device.UpdateNameInApp;
                //写入DeviceAddr
                this.DataShow.DeviceAddr = this.Device.DeviceAddr;
                this.DataShow.IsNewDevice = true;
            }
        }

        public FormTempHumi()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 主界面的加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormTempHumi_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();//遍历系统串口
            Array.Sort(ports);
            comboBoxPort.Items.AddRange(ports);
            comboBoxPort.SelectedIndex = comboBoxPort.Items.Count > 0 ? 0 : -1;

            Dongle.GAP_DeviceInformationEventPackComeEvent += this.DeviceSearch;//new PackComeEventHandler(Device.DataComeUpdate);
            Dongle.GAP_DeviceInitDoneEventPackComeEvent+= (PackComeEventHandler)(delegate{
                this.labelShowStatus.Text = "Dongle初始化完成！"; //更新状态栏               
            });

            TimerCheckUpdate.Enabled = false;
            TimerCheckUpdate.Interval = 10000;//10秒一次
            TimerCheckUpdate.Tick+=TimerCheckUpdate_Tick;            
        }
        
        /// <summary>
        /// 打开USBDongle设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOpenClose_Click(object sender, EventArgs e)
        {
            if (Dongle.IsOpen) 
            { 
                Dongle.DongleClose();
            }
            else
            {
                Dongle.DongleOpen(comboBoxPort.Text);
                if (Dongle.IsOpen)  //打开了才进行写入
                {
                    this.labelShowStatus.Text = "初始化Dongle...";
                    this.TimerCheckUpdate.Start();
                    Dongle.SendCmd(PackageSend.GAP_DeviceInitPack());//Usbdongle Init
                }
            }
            buttonOpenClose.Text = Dongle.IsOpen ? "Close" : "Open";
        }
              
        /// <summary>
        /// 搜到新的设备的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeviceSearch(object sender, PackEventArgs e)   //搜到设备时候的回掉函数
        {
            GAP_DeviceInformationPack Pack = new GAP_DeviceInformationPack(e._pack);
            if (DeviceTable.ContainsKey(TempHumiDevice.ToHexAddrString(Pack.Addr)))//TODO:修改Pack里面的地址显示方式，改为string
            {       //如果已经在哈希表里面，就更新
                ((DeviceDataShowStruct)DeviceTable[TempHumiDevice.ToHexAddrString(Pack.Addr)]).Device.DeviceUpdate(Pack);
            }
            else
            {       //如果没有，就新建
                       //先验证是不是认证的UUID
                byte[] PackDeviceUUID = GAP_DeviceInformationPack.AdvertData_Extraction(Pack.AdvertData, 0x02);//提取UUID信息
                if (PackageReceive.Combine2ByteToUInt16(PackDeviceUUID[1], PackDeviceUUID[0]) == TempHumiDevice.DeviceServiceUUID)
                {       //如果是，才加入设备
                    TempHumiDevice Device = AddDeviceToTable(Pack);                    
                    //名字发生变化就马上写入xml，订阅事件后先手动更新一次
                    Device.DeviceDataChangeEvent += new DeviceDataUpdateDoneEventHandler(XMLDealer.DeviceData_Update);
                    //编写状态栏
                    this.labelShowStatus.Text = "新增一设备   " + DateTime.Now.ToLocalTime().ToString();
                    //如果从来没有出现过，就插入到XML文件当中
                    if (XMLDealer.QueryDevice(Device) == null)
                    {  
                        XMLDealer.XMLInsertDevice(Device);
                    }
                    else
                    {   //如果本来存在，就写入重新出现时时间
                        XMLDealer.UpdateDeviceElememtInXML(Device, "ReCheckInTime", DateTime.Now.ToString());
                        //同时载入旧的名字,触发事件链
                        ((DeviceDataShowStruct)DeviceTable[Device.DeviceAddr]).DataShow.NameInApplication = XMLDealer.ReloadDeviceOldNameInApp(Device);
                    }
                    //手动更新
                    XMLDealer.UpdateDeviceElememtInXML(Device, "NameInDevice", Device.DeviceName);
                 
                }
                
            }
        }

        /// <summary>
        /// 把新增设备加入到Form中的维护数组
        /// </summary>
        private TempHumiDevice AddDeviceToTable(GAP_DeviceInformationPack Pack)
        {
            
            TempHumiDevice Device = new TempHumiDevice(Pack);//新建一个设备，index代表在界面上的位置
            UserConDataShow DataShow = new UserConDataShow();
            DeviceDataShowStruct newStruct = new DeviceDataShowStruct(Device, DataShow);
            DeviceTable.Add(TempHumiDevice.ToHexAddrString(Pack.Addr), newStruct);//加入到Device的哈希表中

            this.flowLayoutPanelMain.Controls.Add(DataShow);//在容器中显示
            DataShow.Visible = true;           
            return Device;  //返回Device对象
            //break;
                
            //return null;//如果循环出来while就返回null
        }        

        /// <summary>
        /// 检查设备有没有消失了，就是隔了一段时间后还是没有扫描到更新的包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimerCheckUpdate_Tick(object sender, EventArgs e)
        {
            Queue<string> DelDeviceQue = new Queue<string>();
            IDictionaryEnumerator DeviceTableEnumerator = DeviceTable.GetEnumerator();
            DictionaryEntry CurrentDeviceEntry;
            while (DeviceTableEnumerator.MoveNext())    //可以用foreach语句
            {   //  获取每个设备的更新间隔，超过10s没有更新就进行操作
                CurrentDeviceEntry = (DictionaryEntry)DeviceTableEnumerator.Current;
                TempHumiDevice CurrentDevice = ((DeviceDataShowStruct)CurrentDeviceEntry.Value).Device;
                UpdateSpan = DateTime.Now - CurrentDevice.LastUpdate;

                if (UpdateSpan.TotalSeconds > UpdateSpanLimit_Second)
                {   //先把界面隐藏，然后把Device对象移除
                    try
                    {
                       // this.DataShowtable[CurrentDevice.DataShowTableIndex].Visible = false;
                        //DeviceTable.Remove(CurrentDevice.DeviceAddr);//不能在迭代的时候进行删除操作
                        DelDeviceQue.Enqueue(CurrentDevice.DeviceAddr);//将要移除的Device的地址入队列
                    }
                    catch
                    {
                        break;
                    }
                }
            }
            while (DelDeviceQue.Count != 0) //将队列里面的设备都删除
            {
                try
                {
                    //先把消失时候的数据保存到XML当中，再移除
                    string tempDeviceAddr = DelDeviceQue.Dequeue();
                    TempHumiDevice tempDevice = ((DeviceDataShowStruct)this.DeviceTable[tempDeviceAddr]).Device as TempHumiDevice;
                    XMLDealer.UpdateDeviceElememtInXML(tempDevice, "LastDisapperTime", DateTime.Now.ToString());
                    XMLDealer.UpdateDeviceElememtInXML(tempDevice, "DisapperRssi", tempDevice.Rssi.ToString());            
                    //先从容器中移除
                    this.flowLayoutPanelMain.Controls.Remove(((DeviceDataShowStruct)this.DeviceTable[tempDeviceAddr]).DataShow);
                    //最后移除出DeviceTable这个哈希表
                    this.DeviceTable.Remove(tempDeviceAddr);
                    //编写状态栏
                    this.labelShowStatus.Text = "一设备消失   " + DateTime.Now.ToLocalTime().ToString();
                }
                catch
                {
                    ;
                }
            }
        }

        /// <summary>
        /// test
        /// 获取串口信息
        /// </summary>
        /// <returns></returns>
        public string[] GetCommList()
        {
            Microsoft.Win32.RegistryKey keyCom = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            string[] sSubkey = keyCom.GetValueNames();
            string[] str = new string[sSubkey.Length];
            for (int i = 0; i < sSubkey.Length; i++)
            {
                str[i] = (string)keyCom.GetValue(sSubkey[i]);
            }
            return str;
        }


        
    }
}
