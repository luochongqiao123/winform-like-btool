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
        private int _DataShowTableIndex;//代表在界面上的第几个显示框
        private string _nameInApplication;//在软件上的命名

        public const UInt16 DeviceServiceUUID = 0xFFF0;

        public event EventHandler DeviceUpdateDoneEvent; //设备更新完以后通知界面来更新
        public event DeviceDataUpdateDoneEventHandler DeviceDataChangeEvent;    //设备名称改变，通知xml来改写
        public event EventHandler DeviceNameInAppChange;    //用户命名出现改变以后的事件

        public string DeviceAddr { get { return this._addr; } }
        public byte Rssi { get { return this._currentRssi; } }
        public double CurTemp { get { return _currentTemp; } }
        public double CurHumi { get { return _currentHumi; } }
        public string DeviceName { get { return _devicename; } }
        public DateTime LastUpdate { get { return this._lastUpdate; } }
        public int DataShowTableIndex { get { return this._DataShowTableIndex; } }
        public string NameInApplication { 
            get { return this._nameInApplication; }
            set 
            { 
                this._nameInApplication = value;
                if (this.DeviceNameInAppChange != null)
                {   //改名触发事件
                    DeviceNameInAppChange(this, new EventArgs());
                }
            }
        }

        public TempHumiDevice(GAP_DeviceInformationPack Pack,int DataShowTableIndex)
        {
            this._addr = ToHexAddrString(Pack.Addr);
            this._lastUpdate = new DateTime();
            this._lastUpdate = DateTime.Now;
            this._lastRssi = this._currentRssi = Pack.Rssi;
            this._DataShowTableIndex = DataShowTableIndex;
        }

        public void DeviceUpdate(GAP_DeviceInformationPack Pack)
        {
            if (ToHexAddrString(Pack.Addr) != this._addr) return;
            this._lastRssi = this._currentRssi;
            this._currentRssi = Pack.Rssi;
            this._lastUpdate = DateTime.Now;
            if (Pack.EventType == GAP_DeviceInformationPack.Undirect_Advertisement)
            {   //  更新温湿度数据                
                byte[] temp1 = GAP_DeviceInformationPack.AdvertData_Extraction(Pack.AdvertData, 0x16);
                const double d1 = -40.1;
                const double d2 = 0.01;
                UInt16 SOt = PackageReceive.Combine2ByteToUInt16(temp1[0], temp1[1]);
                _currentTemp = d1 + d2 * SOt;//计算得到温度

                const double c1 = -2.0468;
                const double c2 = 0.0367;
                const double c3 = -1.5955E-6;
                UInt16 SOrh = PackageReceive.Combine2ByteToUInt16(temp1[3], temp1[4]);
                _currentHumi = c1 + c2 * SOrh + c3 * SOrh * SOrh;//计算得到湿度
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


        //private void ProcessData(byte[] Data, byte eventType)//把广播的数据进行分段分析，提取有用的信息
        //{
        //    if (Data == null) return;

        //    int index = 0;
        //    int length = Data[index];
        //    byte targetParam;//数据段的类型
        //    byte currentParam = Data[index+1];

        //    switch (eventType)
        //    {
        //        case GAP_DeviceInformationPack.scanRsp:
        //            targetParam = 0x09;//device name
        //            while (index<Data.Length)
        //            {
        //                if (targetParam == currentParam)
        //                {
        //                    break;//如果相同，当前index就是所需的数据段
        //                }
        //                else
        //                {
        //                    try
        //                    {
        //                        index += length + 1;//如果不相同，就往下移
        //                        length = Data[index];
        //                        currentParam = Data[index + 1];
        //                    }
        //                    catch
        //                    {
        //                        break;
        //                    }
        //                }
        //            }//end of while

        //            if (index < Data.Length)//如果有找到数据段
        //            {
        //                byte[] devicename = new byte[length - 1];
        //                Array.Copy(Data, index + 2, devicename, 0, length - 1);//复制到temp byte[]
        //                this._devicename = System.Text.ASCIIEncoding.ASCII.GetString(devicename);
        //            }

        //            break;

        //        case GAP_DeviceInformationPack.Undirect_Advertisement:
        //            targetParam = 0x16;//service data
        //            while (index < Data.Length)
        //            {
        //                if (targetParam == currentParam)
        //                {
        //                    break;//如果相同，当前index就是所需的数据段
        //                }
        //                else
        //                {
        //                    try
        //                    {
        //                        index += length + 1;//如果不相同，就往下移
        //                        length = Data[index];
        //                        currentParam = Data[index + 1];
        //                    }
        //                    catch
        //                    {
        //                        break;
        //                    }
        //                }
        //            }//end of while

        //            if (index < Data.Length)//如果有找到数据段
        //            {
        //                const double d1 = -40.1;
        //                const double d2 = 0.01;
        //                UInt16 SOt = PackageReceive.Combine2ByteToUInt16(Data[index + 2], Data[index + 3]);
        //                _currentTemp = d1 + d2 * SOt;//计算得到温度

        //                const double c1 = -2.0468;
        //                const double c2 = 0.0367;
        //                const double c3 = -1.5955E-6;
        //                UInt16 SOrh = PackageReceive.Combine2ByteToUInt16(Data[index + 5], Data[index + 6]);
        //                _currentHumi = c1 + c2 * SOrh + c3 * SOrh * SOrh;//计算得到湿度
        //            }
        //            break;

        //        default: break;
        //    }//end of switch case           
        //}

        //public void DataComeUpdate(object sender, PackEventArgs e)//收到新包的事件处理函数
        //{
        //    DeviceUpdate(new GAP_DeviceInformationPack(e._pack));
        //}    
        
    }
}
