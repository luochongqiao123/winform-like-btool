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
            //String Buffer = comm.ReadExisting();
            //Buffer.ToArray();
            comm.Read(buf, 0, n);
            //GAP_HCI_ExtertionCommandStatusPack Pack = new GAP_HCI_ExtertionCommandStatusPack(buf);
            if (PackageReceive.Combine2ByteToUInt16(buf[4], buf[3]) == 0x0600)
            {
                GAP_DeviceInitDonePack Pack = new GAP_DeviceInitDonePack(buf);
            }
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
                comm.Write(package, 0, package.Length);
            //comm.Write()
            n = textBoxSend.Text.Length + 2;
            SendCount += n;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            SendCount = 0;
        }




        
    }
}
