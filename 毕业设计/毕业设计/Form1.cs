using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace 毕业设计
{
    public partial class FormTempHumi : Form
    {

        //private SerialPort comm;//USBDongle的串口
        Queue<byte[]> PackQue = new Queue<byte[]>();
        UsbDongle Dongle = new UsbDongle();
        TempHumiDevice Device = new TempHumiDevice();

        public FormTempHumi()
        {
            InitializeComponent();
        }

        private void FormTempHumi_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();//遍历系统串口
            Array.Sort(ports);
            comboBoxPort.Items.AddRange(ports);
            comboBoxPort.SelectedIndex = comboBoxPort.Items.Count > 0 ? 0 : -1;

            Dongle.GAP_DeviceInformationEventPackComeEvent += new PackComeEventHandler(Device.DataComeUpdate) + new PackComeEventHandler(this.UpdateConShow);

            /*
            this.userConDataShow1.DeviceAddr = "00:01:00:02:03:05";
            this.userConDataShow1.Temp = "20";
            this.userConDataShow1.Humi = "90";
            this.userConDataShow1.Rssi = "210";
            */
        }
      
        private void buttonOpenClose_Click(object sender, EventArgs e)
        {
            if (Dongle.IsOpen) 
            { 
                Dongle.DongleClose();
            }
            else
            {
                Dongle.DongleOpen(comboBoxPort.Text);
                if (Dongle.IsOpen)  //打开了才进行写入
                {
                    Dongle.SendCmd(PackageSend.GAP_DeviceInitPack());//Usbdongle Init
                }
            }
            buttonOpenClose.Text = Dongle.IsOpen ? "Close" : "Open";
        }

        void UpdateConShow(object sender, PackEventArgs e)
        {
            this.userConDataShow1.DeviceAddr = Device.DeviceAddr;
            this.userConDataShow1.Temp = Device.CurTemp.ToString()+"℃";
            this.userConDataShow1.Humi = Device.CurHumi.ToString("n2")+"%";
            this.userConDataShow1.Rssi = Device.Rssi.ToString("X2");
        }

    }
}
