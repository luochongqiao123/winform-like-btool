using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 串口操作
{
    /*
     * 数据包的基类
     */
    class PackageReceive
    {
        private const byte _type = 0x04;
        private byte _eventcode;
        private byte _datalength;
        private UInt16 _event;
        protected byte[] DataList;
        

        public byte EventCode { get { return _eventcode; } }
        public byte PackDataLength { get { return _datalength; } }
        public UInt16 Event { get { return _event; } }

        public const byte Success = 0x00;

        public PackageReceive(byte[] DataBuf)
        {
            _eventcode = DataBuf[1];    //一般为0xFF
            _datalength = DataBuf[2];
            _event = (UInt16)(DataBuf[4] << 8); //位于数据包的头部后的前两个字节
            _event |= DataBuf[3];
            this.DataList = DataBuf;//直接把引用赋值到类里面
        }

        public byte this[int idx]
        {
            get
            {
                if (idx >= 0 && idx < DataList.Length)
                {
                    return DataList[idx];
                }
                else
                    return 0x00;
            }
        }

        /*
         * 把两个8bit的byte组合成一个16bit的UInt16
         */
        public static UInt16 Combine2ByteToUInt16(byte value1, byte value2)
        {
            UInt16 temp1 = (UInt16)value1;
            temp1 = (UInt16)(temp1 << 8);
            return (UInt16)(temp1 | value2);
        }
    }

    /*
     * 握手包的实现
     */
    class GAP_HCI_ExtertionCommandStatusPack : PackageReceive, IGAP_HCI_ExtertionCommandStatus
    {
        private byte[] _data;

        public const UInt16 GAP_GetParamOpCode = 0xFE31;
        public const UInt16 GAP_SetParamOpCode = 0xFE30;

        public GAP_HCI_ExtertionCommandStatusPack(byte[] DataBuf)
            : base(DataBuf)
        {
            if (this.DataLength != 0)
            {
                _data = new byte[DataLength];   //把数据复制到另外一个数组里面
                Array.Copy(this.DataList, 9, _data, 0, this.DataLength);
            }
            else
                _data = null;
        }
        public byte Status { get { return this.DataList[5]; } }
        public UInt16 OpCode { get { return PackageReceive.Combine2ByteToUInt16(DataList[7],DataList[6]); } }
        public byte DataLength { get { return this.DataList[8]; } }
        public byte[] Data { get { return _data; } }
    }

    /*
     * Dongle初始化完成的包
     */
    class GAP_DeviceInitDonePack : PackageReceive, IGAP_DeviceInitDone
    {
        //private byte[] buf;

        public GAP_DeviceInitDonePack(byte[] buf) 
            : base(buf)
        {
            // TODO: Complete member initialization
        }
        public byte Status { get { return this.DataList[5]; } }
        public byte[] DevAddr
        {
            get
            {
                byte[] temp = new byte[6];
                Array.Copy(this.DataList, 6, temp, 0, 6);
                Array.Reverse(temp);
                return temp;
            }
        }
    }

    /*
     * 广播后发现设备的包
     */
    class GAP_DeviceInformationPack : PackageReceive, IGAP_DeviceInformation
    {
        private byte[] _data;

        public const byte scanRsp = 0x04;
        public const byte Undirect_Advertisement = 0x00;

        public GAP_DeviceInformationPack(byte[] buf)
            : base(buf)
        {
            //TODO Copy the AdverData while Init
            if (this.DataLength != 0)
            {
                _data = new byte[this.DataLength];
                Array.Copy(this.DataList, 16, _data, 0, this.DataLength);
                //Array.Reverse(_data);
            }
        }

        public byte Status { get { return this.DataList[5]; } }
        public byte EventType { get { return this.DataList[6]; } }
        public byte[] Addr
        {
            get
            {
                byte[] temp = new byte[6];
                Array.Copy(this.DataList, 8, temp, 0, 6);
                Array.Reverse(temp);
                return temp;
            }
        }
        public byte Rssi { get { return this.DataList[14]; } }
        public byte DataLength { get { return this.DataList[15]; } }
        public byte[] Data { get { return _data; } }
    }

    /*
     * 扫描以后的设备汇总的包
     */
    class GAP_DeviceDiscoveryDonePack : PackageReceive, IGAP_DeviceDiscoveryDone
    {
        public GAP_DeviceDiscoveryDonePack(byte[] buf)
            : base(buf)
        {
            //TODO
        }

        public byte Status { get { return this.DataList[5]; } }
        public byte NumDevs { get { return this.DataList[6]; } }
    }

}










