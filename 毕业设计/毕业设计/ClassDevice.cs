﻿using System;
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
            ProcessData(Pack.Data, GAP_DeviceInformationPack.Undirect_Advertisement);
        }

        public string DeviceAddr { get { return this._addr; } }
        public byte Rssi { get { return this._currentRssi; } }
        public double CurTemp { get { return _currentTemp; } }
        public double CurHumi { get { return _currentHumi; } }

        public static string ToHexString(byte[] data)//把byte[]转换为16进制的string地址类型
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
            if (Data == null) return;

            int index = 0;
            int length = Data[index];
            byte targetParam;//数据段的类型
            byte currentParam = Data[index+1];


            switch (eventType)
            {
                case GAP_DeviceInformationPack.scanRsp:
                    targetParam = 0x09;//device name
                    while (index<Data.Length)
                    {
                        if (targetParam == currentParam)
                        {
                            break;//如果相同，当前index就是所需的数据段
                        }
                        else
                        {
                            index += length+1;//如果不相同，就往下移
                            length = Data[index];
                            currentParam = Data[index + 1];
                        }
                    }

                    if (index < Data.Length)//如果有找到数据段
                    {
                        byte[] devicename = new byte[length - 1];
                        Array.Copy(Data, index + 2, devicename, 0, length - 1);//复制到temp byte[]
                        this._devicename = System.Text.ASCIIEncoding.ASCII.GetString(devicename);
                    }

                    break;

                case GAP_DeviceInformationPack.Undirect_Advertisement:
                    targetParam = 0x16;//service data
                    while (index < Data.Length)
                    {
                        if (targetParam == currentParam)
                        {
                            break;//如果相同，当前index就是所需的数据段
                        }
                        else
                        {
                            index += length + 1;//如果不相同，就往下移
                            length = Data[index];
                            currentParam = Data[index + 1];
                        }
                    }

                    if (index < Data.Length)//如果有找到数据段
                    {
                        
                    }
                    break;

                default: break;
            }


            
        }
       
        
    }
}
