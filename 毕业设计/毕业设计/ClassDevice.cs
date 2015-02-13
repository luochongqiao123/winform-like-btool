using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 毕业设计
{
    class TempHumiDevice
    {
        private string _addr;
        private string _devicename; //固件中的命名
        private byte _currentRssi;
        private byte _lastRssi;
        private DateTime _lastUpdate;
        private double _currentTemp;
        private double _currentHumi;
        private double _vccVoltage;
        private string _nameInApplication;//在软件上的命名

        public const UInt16 DeviceServiceUUID = 0xFFF0;

        public event EventHandler DeviceUpdateDoneEvent; //设备更新完以后通知界面来更新
        public event DeviceDataUpdateDoneEventHandler DeviceDataChangeEvent;    //设备名称改变，通知xml来改写
        //public event DeviceDataUpdateDoneEventHandler DeviceNameInAppChange;    //用户命名出现改变以后的事件

        public string DeviceAddr { get { return this._addr; } }
        public byte Rssi { get { return this._currentRssi; } }
        public double CurTemp { get { return _currentTemp; } }
        public double CurHumi { get { return _currentHumi; } }
        public string DeviceName { get { return _devicename; } }
        public DateTime LastUpdate { get { return this._lastUpdate; } }
        public string NameInApplication { 
            get { return this._nameInApplication; }
            set 
            { 
                this._nameInApplication = value;
                if (this.DeviceDataChangeEvent != null)
                {   //改名触发事件
                    DeviceDataChangeEvent(this, new DeviceEventArgs("NameInApp"));
                }
            }
        }
        public double VccVoltage { get { return _vccVoltage; } }

        /// <summary>
        /// 初始化，写入设备地址等数据
        /// </summary>
        /// <param name="Pack"></param>
        public TempHumiDevice(GAP_DeviceInformationPack Pack)
        {
            this._addr = ToHexAddrString(Pack.Addr);
            this._lastUpdate = new DateTime();
            this._lastUpdate = DateTime.Now;
            this._lastRssi = this._currentRssi = Pack.Rssi;           
        }

        /// <summary>
        /// 数据更新，收到数据包后根据包的类型更新Device对象的数据
        /// </summary>
        /// <param name="Pack"></param>
        public void DeviceUpdate(GAP_DeviceInformationPack Pack)
        {
            if (ToHexAddrString(Pack.Addr) != this._addr) return;
            this._lastRssi = this._currentRssi;
            this._currentRssi = Pack.Rssi;
            this._lastUpdate = DateTime.Now;
            if (Pack.EventType == GAP_DeviceInformationPack.Undirect_Advertisement)
            {   //  更新温湿度数据  
                try
                {
                    byte[] temp1 = GAP_DeviceInformationPack.AdvertData_Extraction(Pack.AdvertData, 0x16);
                    //先计算电源电压
                    UInt16 tempVoltage = PackageReceive.Combine2ByteToUInt16(temp1[6], temp1[7]);
                    _vccVoltage = (double)tempVoltage * 1.15 / 8192 * 3;

                    const double d1 = -39.6;
                    const double d2 = 0.01;
                    UInt16 SOt = PackageReceive.Combine2ByteToUInt16(temp1[0], temp1[1]);
                    _currentTemp = d1 + d2 * SOt;//计算得到温度

                    const double c1 = -2.0468;
                    const double c2 = 0.0367;
                    const double c3 = -1.5955E-6;
                    UInt16 SOrh = PackageReceive.Combine2ByteToUInt16(temp1[3], temp1[4]);
                    _currentHumi = c1 + c2 * SOrh + c3 * SOrh * SOrh;//计算得到湿度
                }
                catch
                {
                    ;//TODO
                }
            }
            else if (Pack.EventType == GAP_DeviceInformationPack.scanRsp)
            {   //更新设备名称数据
                byte[] temp2 = GAP_DeviceInformationPack.AdvertData_Extraction(Pack.AdvertData, 0x09);
                string oldDeviceName = this._devicename;    //旧名字用来比较有没有变
                this._devicename = System.Text.ASCIIEncoding.ASCII.GetString(temp2);//取得名字
                if (this._devicename != oldDeviceName)
                {
                    if (this.DeviceDataChangeEvent != null)
                    {   //设备名称发生改变就通知
                        DeviceDataChangeEvent(this,new DeviceEventArgs("NameInDevice"));
                    }
                }
            }

            if (this.DeviceUpdateDoneEvent != null)
            {//设备更新完以后通知界面来更新
                DeviceUpdateDoneEvent(this, new EventArgs());
            }
        }

        /// <summary>
        /// 将byte[]的设备地址转换为string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToHexAddrString(byte[] data)//把byte[]转换为16进制的string地址类型
        {
            string HexString = string.Empty;
            if (data != null)
            {
                StringBuilder strB = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    strB.Append(data[i].ToString("X2"));
                    strB.Append(":");//中间插入：
                }
                strB.Remove(strB.Length - 1, 1);//把最后的：冒号去掉
                HexString = strB.ToString();
            }
            return HexString;
        }

        /// <summary>
        /// 界面上名称改变（NameInApp）就修改Device对象的name，同时触发事件，写入XML中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateNameInApp(object sender, EventArgs e)
        {   //在界面上改变名字后触发到这里来
            UserConDataShow Shower = sender as UserConDataShow;
            this.NameInApplication = Shower.NameInApplication;//设置属性同时会触发事件
        }

         
        
    }
}
