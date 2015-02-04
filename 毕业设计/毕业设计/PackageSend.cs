using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 毕业设计
{
    static class PackageSend
    {
        /*初始化设备命令
         * 
         */
        private static byte[] GAP_DeviceInit = 
            new byte[]{0x01,0x00,0xFE ,0x26 ,0x08 ,0x05 ,0x00 ,0x00 ,0x00 ,
                0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,
                0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,
                0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,
                0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x01 ,0x00 ,0x00 ,0x00 };
        public static byte[] GAP_DeviceInitPack()
        {
            return GAP_DeviceInit;
        }
        public static int GAP_DeviceInitLength
        {
            get { return GAP_DeviceInit.Length; }
        }


        public const byte TGAP_GEN_DISC_SCAN = 2;  //!< Minimum time to perform scanning, when performing General Discovery proc (mSec)
        public const byte TGAP_LIM_DISC_SCAN = 3;  //!< Minimum time to perform scanning, when performing Limited Discovery proc (mSec)
        /*GAP_GetParam命令
         * 用于获取扫描设备的参数
         */
        public const int GAP_GetParamLength = 5;
        private static byte[] GAP_GetParam = new byte[] { 0x01, 0x31, 0xFE, 0x01, 0x00 };
        public static byte[] GAP_GetParamPack(byte ParamID)
        {
            GAP_GetParam[4] = ParamID;
            return GAP_GetParam;
        }

        /*GAP_SetParam命令
         * 用于设置扫描设备的参数
         * 
         */
        public const int GAP_SetParamLength = 7;
        private static byte[] GAP_SetParam = new byte[] { 0x01, 0x30, 0xFE, 0x03, 0x00, 0x00, 0x00 };
        public static byte[] GAP_SetParamPack(byte ParamID, UInt16 ParamValue)
        {
            GAP_SetParam[4] = ParamID;
            GAP_SetParam[5] = (byte)(ParamValue & 0xFF);
            GAP_SetParam[6] = (byte)(ParamValue >> 8);
            return GAP_SetParam;
        }

        /*GAP_DeviceDiscoveryRequest命令
         * 扫描附近的广播设备
         * 
         */
        public const int GAP_DeviceDiscoveryRequestLength = 7;
        private static byte[] GAP_DeviceDiscoveryRequest = new byte[] { 0x01, 0x04, 0xFE, 0x03, 0x03, 0x01, 0x00 };
        public static byte[] GAP_DeviceDiscoveryRequestPack()
        {
            return GAP_DeviceDiscoveryRequest;
        }



        //private const byte _Type = 0x01;
        //private UInt16 _OpCode;
        //private byte _DataLength;
        //private byte[] Package;
        //public UInt16 OpCode { get { return _OpCode; } }
        //public byte DataLength { get { return _DataLength; } }
        //public int Length { get { return Package.Length; } }
        //public PackageSend(UInt16 OpCode,byte[] DataBuf)
        //{
        //    this._OpCode = OpCode;
        //    this._DataLength = (byte)DataBuf.Length;

        //    Package = new byte[DataBuf.Length + 4];
        //    Package[0] = _Type;
        //    Package[1] = (byte)(OpCode & 0xFF);
        //    Package[2] = (byte)(OpCode >> 8);
        //    Package[3] = (byte)DataBuf.Length;
        //    DataBuf.CopyTo(Package, 4);
        //}

        //public byte this[int idx]
        //{
        //    get
        //    {
        //        if (idx >= 0 && idx < Package.Length)
        //        {
        //            return Package[idx];
        //        }
        //        else
        //        {
        //            return 0xFF;
        //        }
                    
        //    }
        //}

    }
}






