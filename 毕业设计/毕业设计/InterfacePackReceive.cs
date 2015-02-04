using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 毕业设计
{
    /*
     * 发送数据包以后Dongle发回来的握手包，表示数据包的接受情况
     */
    interface IGAP_HCI_ExtertionCommandStatus
    {
         byte Status { get; }
         UInt16 OpCode { get; }
         byte DataLength { get; }
         byte[] Data { get; }
    }

    /*
     * Dongle初始化以后的回传包，表示初始化以后的各种信息
     */
    interface IGAP_DeviceInitDone
    {
         byte Status { get; }
         byte[] DevAddr { get; }
         //const byte AddrLength = 6;
    }

    /*
     * 扫描以后发现设备的各种数据
     */
    interface IGAP_DeviceInformation
    {
         byte Status { get; }
         byte EventType { get; }
         byte[] Addr { get; }
         byte Rssi { get; }
         byte DataLength { get; }
         byte[] AdvertData { get; }
    }

    /*
     * 扫描完成以后的设备汇总
     */
    interface IGAP_DeviceDiscoveryDone
    {
         byte Status { get; }
         byte NumDevs { get; }
    }

}












