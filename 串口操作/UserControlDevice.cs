using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 串口操作
{
    public partial class UserControlDevice : UserControl
    {
        public UserControlDevice()
        {
            InitializeComponent();
        }

        //public UserControlDevice(GAP_DeviceInformationPack Pack)
        //{
        //    InitializeComponent();
        //    //this.labelDeviceAddr.Text = Pack.Addr.ToString();
        //}


        private void UserControlDevice_Load(object sender, EventArgs e)
        {

        }

        public string Addr { get { return this.labelDeviceAddr.Text;} }
    }
}
