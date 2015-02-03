using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 串口操作
{
    class TempHumiDevice
    {
        private string _addr;
        private string _devicename;
        private byte _currentRssi;
        private byte _lastRssi;
        private DateTime _lastUpdate;
        private double _currentTemp;
        private double _currentHumi;

        public TempHumiDevice(GAP_DeviceInformationPack Pack)
        {
            this._addr = ToHexString(Pack.Addr);
            this._lastUpdate = new DateTime();
            this._lastUpdate = DateTime.Now;
            this._lastRssi = this._currentRssi = Pack.Rssi;
        }

        public void DeviceUpdate(GAP_DeviceInformationPack Pack)
        {
            if (ToHexString(Pack.Addr) != this._addr) return;
            this._lastRssi = this._currentRssi;
            this._currentRssi = Pack.Rssi;
            this._lastUpdate = DateTime.Now;
        }

        public string DeviceAddr { get { return this._addr; } }
        public byte Rssi { get { return this._currentRssi; } }
        //public 

        public static string ToHexString(byte[] data)//把byte[]转换为16进制的string
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
                strB.Remove(strB.Length - 1, 1);//把最后的：（）冒号去掉
                HexString = strB.ToString();
            }
            return HexString;
        }

        private void ProcessData(byte[] Data, byte eventType)//把广播的数据进行分段分析，提取有用的信息
        {
            byte targetParam;
            if (eventType == GAP_DeviceInformationPack.scanRsp) targetParam = 0x09;//device name
            if (eventType == GAP_DeviceInformationPack.Undirect_Advertisement) targetParam = 0x16;//service data
            
            int index = 0;
            int length = (int)Data[index + 1];
            while (index < Data.Length)
            {
                if()
            }
        }
       
        
    }
}
