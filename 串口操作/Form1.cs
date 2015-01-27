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
using System.Text.RegularExpressions;

namespace 串口操作
{
    public partial class FormPort : Form
    {
        private SerialPort comm = new SerialPort();//串口对象
        private StringBuilder builder = new StringBuilder();//

        private Queue<byte[]> BufferStream = new Queue<byte[]>();

        private long ReceivedCount = 0;
        private long SendCount = 0;

        private static byte TickCount = 0;

        public const UInt16 GAP_HCI_ExtentionCommandStatusEvent = 0x067F;
        public const UInt16 GAP_DeviceInitDoneEvent = 0x0600;
        public const UInt16 GAP_DeviceInformationEvent = 0x060D;
        public const UInt16 GAP_DeviceDiscoveryDoneEvent = 0x0601;
        

        public FormPort()
        {
            InitializeComponent();
        }

        private void FormPort_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);
            comboBoxPortName.Items.AddRange(ports);
            comboBoxPortName.SelectedIndex = comboBoxPortName.Items.Count > 0 ? 0 : -1;
            comm.NewLine = "\r\n";
            comm.RtsEnable = true;
            //添加注册事件

            comm.DataReceived += new SerialDataReceivedEventHandler(comm_DataReceived);
            //this.reportViewer1.RefreshReport();
        }

         void comm_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            SerialPort comm = (SerialPort)sender;
            int n = comm.BytesToRead;
            
            byte[] buf = new byte[n];
            ReceivedCount += n;
            comm.Read(buf, 0, n);
            BufferStream.Enqueue(buf);
            //String Buffer = comm.ReadExisting();
            //Buffer.ToArray();
            //GAP_HCI_ExtertionCommandStatusPack Pack = new GAP_HCI_ExtertionCommandStatusPack(buf);
            //if (PackageReceive.Combine2ByteToUInt16(buf[4], buf[3]) == 0x0600)
            //{
            //    GAP_DeviceInitDonePack Pack = new GAP_DeviceInitDonePack(buf);
            //}
            builder.Clear();
            

            this.Invoke((EventHandler)(delegate
            {
                foreach (byte b in buf)
                {
                    builder.Append(b.ToString("X2") + " ");
                }
                this.textBoxGet.AppendText(builder.ToString()+"\r\n");
            }));
        }

        private void buttonOpenClose_Click(object sender, EventArgs e)
        {
            if (comm.IsOpen)
            {
                comm.Close();
            }
            else
            {
                comm.PortName = comboBoxPortName.Text;
                comm.BaudRate = 115200;
                try
                {
                    comm.Open();
                    //comm.Write(PackageSend.GAP_DeviceInit,0,PackageSend.GAP_DeviceInit.Length);
                    comm.Write(PackageSend.GAP_DeviceInitPack(), 0, PackageSend.GAP_DeviceInitLength);
                   // comm.Write(PackageSend.GAP_GetParamPack(PackageSend.TGAP_GEN_DISC_SCAN),0,PackageSend.GAP_GetParamLength);
                    timerCheak.Start();
                }
                catch(Exception ex)
                {
                    comm = new SerialPort();
                    MessageBox.Show(ex.Message);
                }
            }
            buttonOpenClose.Text = comm.IsOpen ? "Close" : "Open";
            buttonSend.Enabled = comm.IsOpen;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            int n = 0;
            byte[] package = { 0x01, 0x04, 0xFE, 0x03, 0x03, 0x01, 0x00 };
            //PackageSend Package = new PackageSend(0xFE04, new byte[3] {   0x03, 0x01, 0x00 });
            
                //comm.WriteLine(textBoxSend.Text);
                //comm.Write(package, 0, package.Length);
            //comm.Write()
            comm.Write(PackageSend.GAP_SetParamPack(0x02, 0x07D0),0,PackageSend.GAP_SetParamLength);
            n = textBoxSend.Text.Length + 2;
            SendCount += n;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            SendCount = 0;
        }


        private void timerCheak_Tick(object sender, EventArgs e)
        {
            if (TickCount++ >= 3)   //每三秒扫描一次
            {
                TickCount = 0;
                comm.Write(PackageSend.GAP_DeviceDiscoveryRequestPack(), 0, PackageSend.GAP_DeviceDiscoveryRequestLength);
            }

            PackageProcess();   //每秒都检查一次缓存里面有没有数据需要处理

        }

        private void PackageProcess()
        {
            while (this.BufferStream.Count != 0)
            {
                this.labelQueueCount.Text = this.BufferStream.Count.ToString();
                byte[] temp = this.BufferStream.Dequeue();
                if (temp.Length <= 4) break;
                switch (PackageReceive.Combine2ByteToUInt16(temp[4],temp[3]))
                {
                    case GAP_HCI_ExtentionCommandStatusEvent:
                        //TODO
                        break;

                    case GAP_DeviceInitDoneEvent:
                        //TODO
                        break;

                    case GAP_DeviceInformationEvent:
                        //TODO
                        GAP_DeviceInformationProcess(new GAP_DeviceInformationPack(temp));
                        break;

                    case GAP_DeviceDiscoveryDoneEvent:
                        //TODO
                        break;

                    default: break;

                }
            }
        }

        /*
         * 处理回发包的函数
         */
        private void GAP_HCI_ExtentionCommandStatusProcess(GAP_HCI_ExtertionCommandStatusPack Pack)
        {
            if (Pack.Status != PackageReceive.Success) return;
        }

        /*
         * 处理扫描以后发回的数据
         */
        private void GAP_DeviceInformationProcess(GAP_DeviceInformationPack Pack)
        {
            if (Pack.EventType == 0x04)
            {
                byte[] temp = Pack.Data;
                StringBuilder newBuilder = new StringBuilder();
                //this.textBoxAdvertData
                foreach (byte b in temp)
                {
                    newBuilder.Append(b.ToString("X2") + " ");
                }
                this.textBoxAdvertData.AppendText(newBuilder.ToString() + "\r\n");
            }

        }
        
    }
}
