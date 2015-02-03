using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 串口操作
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
}
