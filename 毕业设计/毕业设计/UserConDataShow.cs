using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 毕业设计
{
    public partial class UserConDataShow : UserControl
    {
        public UserConDataShow()
        {
            InitializeComponent();
        }

        private string _addr;
        private string _temp;
        private string _humi;
        private string _name;
        private string _rssi;

        public string DeviceAddr
        {
            set
            {
                _addr = value;
                this.labelDeviceAddr.Text = _addr;
                //TODO:用正则表达式判断是否符合地址格式
            }

            get
            {
                return _addr;
            }
        }

        public string Temp
        {
            set
            {
                _temp = value;
                this.labelTemp.Text = _temp;
            }

            get
            {
                return _temp;
            }
        }

        public string Humi
        {
            set
            {
                _humi = value;
                this.labelHumi.Text = _humi;
            }

            get
            {
                return _humi;
            }
        }

        public string DeviceName
        {
            set
            {
                _name = value;
                this.labelDeviceName.Text = _name;
            }

            get
            {
                return _name;
            }
        }

        public string Rssi
        {
            set
            {
                _rssi = value;
                this.labelRssi.Text = _rssi;
            }

            get
            {
                return _rssi;
            }
        }
    }
}
