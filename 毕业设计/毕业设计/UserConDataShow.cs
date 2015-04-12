using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace 毕业设计
{
    public partial class UserConDataShow : UserControl
    {
        public UserConDataShow()
        {
            InitializeComponent();
           // this.ContextMenu = this.contextMenuStripDataShow;
            _TempData = new Queue<double>();
            _HumiData = new Queue<double>();
        }

        private string _addr;
        private double _temp;
        private double _humi;
        private string _name;
        private string _rssi;
        private DateTime _deviceLastUpdate;
        private string _nameInapp;//在软件上显示的名称
        private string _vccVoltage; //电源电压
        private Queue<double> _TempData;  //缓存数据，用于曲线图
        private Queue<double> _HumiData;

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
        public double Temp
        {
            set
            {
                _temp = value;
                //加入缓存
                _TempData.Enqueue(_temp);
                //保持只有10个数据
                while (_TempData.Count > 20)
                    _TempData.Dequeue();
                this.labelTemp.Text = _temp.ToString() + "℃";
            }

            get
            {
                return _temp;
            }
        }
        public double Humi
        {
            set
            {
                _humi = value;
                _HumiData.Enqueue(_humi);
                //保持只有十个数据
                while (_HumiData.Count > 10)
                    _HumiData.Dequeue();
                this.labelHumi.Text = _humi.ToString("n2") + "%";
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
        public string VccVoltage
        {
            get { return _vccVoltage; }
            set 
            { 
                this._vccVoltage = value;
                this.labelVccVoltage.Text = this._vccVoltage;
            }
        }
        

        private FormShowTempHumiTable FormShowTable;//弹窗

        /// <summary>
        /// 更新界面上的数据和信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataShowUpdate(object sender, EventArgs e)  //搭配Device，更新显示的数据
        {
            TempHumiDevice Device = (TempHumiDevice)sender;

            this.Invoke((EventHandler)(delegate
            {
                this.Temp = Device.CurTemp;//温度                
                this.Humi = Device.CurHumi;//湿度               
                this.Rssi = Device.Rssi.ToString();//Rssi
                this.DeviceName = Device.DeviceName;//设备名称
                this.VccVoltage = Device.VccVoltage.ToString("n2") + "V";//电源电压
                //以下三项都跟工作状态有关
                this.labelWorkStatus.Text = Device.WorkStatus ? "正常工作" : "停止工作";
                this.labelWorkStatus.ForeColor = Device.WorkStatus ? Color.Green : Color.Red;
                //停止工作以后可以隐藏
                this.ToolStripMenuItemDeleteDataShow.Enabled = !Device.WorkStatus;
                //停止工作以后不能显示温度湿度线图
                //this.ToolStripMenuItemShowTable.Enabled = Device.WorkStatus;
                if (!Device.WorkStatus) this.ToolStripMenuItemShowTable.Enabled = false;

                //更新弹窗的数据
                if(this.FormShowTable != null && Device.WorkStatus)
                {
                    //TODO
                    double[] tempx = new double[_TempData.Count];
                    double[] tempy = new double[_TempData.Count];                   

                    //复制到数组并逆序
                    _TempData.CopyTo(tempx, 0);
                    Array.Reverse(tempx);

                    for (int i = 0; i < _TempData.Count; i++)
                    {
                        tempy[i] = (double)i;
                    }
                    FormShowTable.DrawNewData(tempx, tempy, null, null);
                }

            }));
            this.Visible = true;//显示界面
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

        /// <summary>
        /// 显示新增的字样，点击后消失
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelNewDevice_Click(object sender, EventArgs e)
        {
            if (labelNewDevice.Visible == true)
            {
                labelNewDevice.Visible = false;
            }
        }

        /// <summary>
        /// 点击后隐藏显示板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemDeleteDataShow_Click(object sender, EventArgs e)
        {
            //简单的把显示板隐藏
            this.Visible = false;
        }

        /// <summary>
        /// 弹窗显示温湿度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItemShowTable_Click(object sender, EventArgs e)
        {
            //失能右键按钮
            this.ToolStripMenuItemShowTable.Enabled = false;
            //显示窗口的名字跟设备的名字一样,传入右键按钮控制失能使能
            FormShowTable = new FormShowTempHumiTable(this.NameInApplication, this.ToolStripMenuItemShowTable);
            FormShowTable.Show();
        }

        

        
    }
}
