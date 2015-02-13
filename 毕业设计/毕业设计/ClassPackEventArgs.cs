using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 毕业设计
{

    public delegate void PackComeEventHandler(object sender,PackEventArgs e);
    //相应的包到来时的委托处理函数

    /// <summary>
    /// 接受到相应的包触发，里面包含包的所有字节
    /// </summary>
    public class PackEventArgs : EventArgs
    {
        public readonly byte[] _pack;

        public PackEventArgs(byte[] data)//data就是数据包的完整内容
        {
            this._pack = data;
        }
    }

    public delegate void DeviceDataUpdateDoneEventHandler(object sender, DeviceEventArgs e);

    /// <summary>
    /// 触发xml写入事件，里面包含要xml xelmemt的node的名字
    /// </summary>
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
