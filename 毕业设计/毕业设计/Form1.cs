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

        public FormTempHumi()
        {
            InitializeComponent();
        }

        private void FormTempHumi_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();//遍历系统串口
            Array.Sort(ports);
            comboBoxPort.Items.AddRange(ports);
            comboBoxPort.SelectedIndex = comboBoxPort.Items.Count > 0 ? 0 : -1;

            Dongle.GAP_DeviceInformationEventPackComeEvent += this.DeviceSearch;//new PackComeEventHandler(Device.DataComeUpdate);

            TimerCheckUpdate.Enabled = false;
            TimerCheckUpdate.Interval = 10000;//一分钟一次
            TimerCheckUpdate.Tick+=TimerCheckUpdate_Tick;

            GenerateConDataShow();  //生成界面
        }
      
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
                    this.TimerCheckUpdate.Start();
                    Dongle.SendCmd(PackageSend.GAP_DeviceInitPack());//Usbdongle Init
                }
            }
            buttonOpenClose.Text = Dongle.IsOpen ? "Close" : "Open";
        }

        private void GenerateConDataShow()
        {
            int x, y, width, height, x_dis, y_dis;
            x = 12; y = 41; width = 202; height = 184; x_dis = 40; y_dis = 20;
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 2; c++)
                {
                    UserConDataShow DataShow = new UserConDataShow();
                    DataShow.Left = x + c * (x_dis+height);
                    DataShow.Top = y + r * (y_dis+width);
                    DataShow.Width = width;
                    DataShow.Height = height;
                    DataShow.Visible = false;
                    DataShowtable[r * 2 + c] = DataShow;
                    this.Controls.Add(DataShow);
                }
            }
        }

        void DeviceSearch(object sender, PackEventArgs e)   //搜到设备时候的回掉函数
        {
            GAP_DeviceInformationPack Pack = new GAP_DeviceInformationPack(e._pack);
            if (DeviceTable.ContainsKey(TempHumiDevice.ToHexAddrString(Pack.Addr)))//TODO:修改Pack里面的地址显示方式，改为string
            {       //如果已经在哈希表里面，就更新
                ((TempHumiDevice)DeviceTable[TempHumiDevice.ToHexAddrString(Pack.Addr)]).DeviceUpdate(Pack);
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
                    //如果从来没有出现过，就插入到XML文件当中
                    if (XMLDealer.QueryDevice(Device) == null)
                    {  
                        XMLDealer.XMLInsertDevice(Device);
                    }
                    else
                    {   //如果本来存在，就写入重新出现时时间
                        XMLDealer.UpdateDeviceElememtInXML(Device, "ReCheckInTime", DateTime.Now.ToString());
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
            int index = 0;
            while (index < DataShowtable.Length)
            {
                if (DataShowtable[index].Visible == false)
                {
                    TempHumiDevice Device = new TempHumiDevice(Pack, index);//新建一个设备，index代表在界面上的位置
                    DeviceTable.Add(TempHumiDevice.ToHexAddrString(Pack.Addr), Device);//加入到Device的哈希表中
      
                    Device.DeviceUpdateDoneEvent += DataShowtable[index].DataShowUpdate;//控件订阅Device更新完毕的事件
                    DataShowtable[index].Visible = true;//显示界面

                    return Device;  //返回Device对象
                    break;
                }
                else
                {
                    index++;
                }
            }
            return null;//如果循环出来while就返回null
        }

        void TimerCheckUpdate_Tick(object sender, EventArgs e)
        {
            Queue<string> DelDeviceQue = new Queue<string>();
            IDictionaryEnumerator DeviceTableEnumerator = DeviceTable.GetEnumerator();
            DictionaryEntry CurrentDeviceEntry;
            while (DeviceTableEnumerator.MoveNext())    //可以用foreach语句
            {   //  获取每个设备的更新间隔，超过10s没有更新就进行操作
                CurrentDeviceEntry = (DictionaryEntry)DeviceTableEnumerator.Current;
                TempHumiDevice CurrentDevice = (TempHumiDevice)CurrentDeviceEntry.Value;
                UpdateSpan = DateTime.Now - CurrentDevice.LastUpdate;

                if (UpdateSpan.TotalSeconds > UpdateSpanLimit_Second)
                {   //先把界面隐藏，然后把Device对象移除
                    try
                    {
                        this.DataShowtable[CurrentDevice.DataShowTableIndex].Visible = false;
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
                    string tempDeviceAddr = DelDeviceQue.Dequeue();//先把消失时候的数据保存到XML当中，再移除
                    TempHumiDevice tempDevice = this.DeviceTable[tempDeviceAddr] as TempHumiDevice;
                    XMLDealer.UpdateDeviceElememtInXML(tempDevice, "LastDisapperTime", DateTime.Now.ToString());
                    XMLDealer.UpdateDeviceElememtInXML(tempDevice, "DisapperRssi", tempDevice.Rssi.ToString());
                    this.DeviceTable.Remove(tempDeviceAddr);
                }
                catch
                {
                    ;
                }
            }
        }

        //void DataShowVisible_Change(object sender, EventArgs e) //DataShow变成不可见的时候删除Device对象
        //{
        //    UserConDataShow DataShow = (UserConDataShow)sender;
        //    if (!DataShow.Visible)
        //    {
        //        try
        //        {   //移除DeviceTable里面的Device对象
        //            this.DeviceTable.Remove(DataShow.DeviceAddr);
        //        }
        //        catch
        //        {
        //            ;
        //        }
        //    }
        //}
    }
}
