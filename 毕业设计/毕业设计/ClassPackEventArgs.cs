using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 毕业设计
{

    public delegate void PackComeEventHandler(object sender,PackEventArgs e);
    //相应的包到来时的委托处理函数

    public class PackEventArgs : EventArgs
    {
        public readonly byte[] _pack;

        public PackEventArgs(byte[] data)//data就是数据包的完整内容
        {
            this._pack = data;
        }
    }

    public delegate void DeviceDataUpdateDoneEventHandler(object sender, DeviceEventArgs e);

    public class DeviceEventArgs : EventArgs
    {
        public readonly string UpdateElementXName;

        /// <summary>
        /// 携带要更新的Element的名字
        /// </summary>
        /// <param name="Xname"></param>
        public DeviceEventArgs(string Xname)
        {
            this.UpdateElementXName = Xname;
        }
    }

    

}
