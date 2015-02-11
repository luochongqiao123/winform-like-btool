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
        private DateTime _deviceLastUpdate;
        private string _nameInapp;//在软件上显示的名称

        public event EventHandler NameInAppChangeEvent;//名字改变时写入

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
        public DateTime DeviceLastUpdate 
        {
            get 
            {
                return this._deviceLastUpdate; 
            }
            set 
            { 
                _deviceLastUpdate = value; 
            }
        }
        public string NameInApplication
        {
            get 
            {
                return this._nameInapp;
            }
            set
            {
                this._nameInapp = value;
                this.textBoxNameInApp.Text = this._nameInapp;
                NameInAppChangeEvent(this, new EventArgs());//触发事件，Device对象更新
            }
        }
        public bool IsNewDevice 
        { 
            get 
            { 
                return this.labelNewDevice.Visible; 
            }
            set
            {
                this.labelNewDevice.Visible = value;
            }
        }

        public void DataShowUpdate(object sender, EventArgs e)  //搭配Device，更新显示的数据
        {
            TempHumiDevice Device = (TempHumiDevice)sender;

            this.Temp = Device.CurTemp.ToString() + "℃";
            this.Humi = Device.CurHumi.ToString("n2") + "%";
            this.Rssi = Device.Rssi.ToString();
            this.DeviceAddr = Device.DeviceAddr;
            this.DeviceName = Device.DeviceName;
            this.DeviceLastUpdate = Device.LastUpdate;
        }

        /// <summary>
        /// 按钮用于保存用户命名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonModifyName_Click(object sender, EventArgs e)
        {
            if (this.textBoxNameInApp.Enabled == false)
            {
                this.buttonModifyName.Text = "锁定";
                this.textBoxNameInApp.Enabled = true;
            }
            else 
            {
                this.buttonModifyName.Text = "修改";
                this.textBoxNameInApp.Enabled = false;
                this.NameInApplication = this.textBoxNameInApp.Text;//修改即触发事件
            }
        }

        private void labelNewDevice_Click(object sender, EventArgs e)
        {
            if (labelNewDevice.Visible == true)
            {
                labelNewDevice.Visible = false;
            }
        }
    }
}
