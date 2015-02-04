using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;

namespace 毕业设计
{
    class UsbDongle
    {
        /*接受到相应的包就触发不同的事件*/
        public event PackComeEventHandler GAP_HCI_ExtentionCommandStatusPackComeEvent;
        public event PackComeEventHandler GAP_DeviceInitDoneEventPackComeEvent;
        public event PackComeEventHandler GAP_DeviceInformationEventPackComeEvent;
        public event PackComeEventHandler GAP_DeviceDiscoveryDoneEventPackComeEvent;

        private SerialPort comm;
        private Queue<byte[]> PackQue = new Queue<byte[]>();//收到的包都放到这个队列里面
        private Timer TimerCheck = new Timer();

        public Boolean IsOpen { get { return comm.IsOpen; } }//显示DOngle的串口打开情况


        public UsbDongle()
        {
            comm = new SerialPort();
            comm.NewLine = "\r\n";
            comm.RtsEnable = true;//视个人情况
            comm.DataReceived += new SerialDataReceivedEventHandler(comm_DataReceived);

            TimerCheck.Enabled = true;
            TimerCheck.Interval = 3000;//设定定时器,3秒一次
            TimerCheck.Stop();
            TimerCheck.Tick +=TimerCheck_Tick;

            this.GAP_DeviceInitDoneEventPackComeEvent += UsbDongle_GAP_DeviceInitDoneEventPackComeEvent;
            
        }

        void comm_DataReceived(object sender, SerialDataReceivedEventArgs e)//串口收到数据后的回调函数
        {
            SerialPort Port = (SerialPort)sender;
            int n = Port.BytesToRead;
            byte[] buf = new byte[n];
            Port.Read(buf, 0, n);//读取串口信息到buf里面
            PackQue.Enqueue(buf);//入队列
        }

        public void DongleOpen(string text)   //打开串口
        {
            comm.PortName = text;
            comm.BaudRate = 115200;
            try
            {
                comm.Open();//打开串口后马上初始化USB Dongle
                comm.DiscardInBuffer();
                comm.DiscardOutBuffer();
                //comm.Write(PackageSend.GAP_DeviceInitPack(), 0, PackageSend.GAP_DeviceInitLength);
            }
            catch (Exception ex)
            {
                comm = new SerialPort();
                TimerCheck.Stop();
                MessageBox.Show(ex.Message);
            }
            if (comm.IsOpen) TimerCheck.Start();//打开了才开始定时器
        }

        public void DongleClose()       //关闭串口
        {
            comm.Close();
            TimerCheck.Stop();
        }

        private void PackageProcess()       //处理在队列的包
        {
            while (this.PackQue.Count != 0)
            {
                byte[] temp = this.PackQue.Dequeue();
                if (temp.Length <= 4) break;//不规则的包会被直接抛弃
                switch (PackageReceive.Combine2ByteToUInt16(temp[4], temp[3]))//判断收到的包是什么类型的包
                {                                   //switch case 触发不同的事件处理函数
                    case PackageReceive.GAP_HCI_ExtentionCommandStatusEvent:
                        //TODO
                        if (GAP_HCI_ExtentionCommandStatusPackComeEvent != null)
                        {
                            GAP_HCI_ExtentionCommandStatusPackComeEvent(this, new PackEventArgs(temp));
                        }
                        break;

                    case PackageReceive.GAP_DeviceInitDoneEvent:
                        //TODO
                        if (GAP_DeviceInitDoneEventPackComeEvent != null)
                        {
                            GAP_DeviceInitDoneEventPackComeEvent(this, new PackEventArgs(temp));
                        }
                        break;

                    case PackageReceive.GAP_DeviceInformationEvent:
                        //TODO
                        //GAP_DeviceInformationProcess(new GAP_DeviceInformationPack(temp));
                        if (GAP_DeviceInformationEventPackComeEvent != null)
                        {
                            GAP_DeviceInformationEventPackComeEvent(this, new PackEventArgs(temp));
                        }
                        break;

                    case PackageReceive.GAP_DeviceDiscoveryDoneEvent:
                        //TODO
                        if (GAP_DeviceDiscoveryDoneEventPackComeEvent != null)
                        {
                            GAP_DeviceDiscoveryDoneEventPackComeEvent(this, new PackEventArgs(temp));
                        }
                        break;

                    default: break;

                }
            }
        }

        public void SendCmd(byte[] SendData)    //发送命令
        {
            comm.Write(SendData, 0, SendData.Length);
        }

        void TimerCheck_Tick(object sender, EventArgs e)    //3秒一次
        {
            PackageProcess();       //先处理完所有的包，再发出扫描的信号
            this.SendCmd(PackageSend.GAP_DeviceDiscoveryRequestPack());
        }

        void UsbDongle_GAP_DeviceInitDoneEventPackComeEvent(object sender, PackEventArgs e)//收到初始化完成的包的处理函数
        {
            GAP_DeviceInitDonePack Pack = new GAP_DeviceInitDonePack(e._pack);
            UsbDongle Dongle = (UsbDongle)sender;
            if (Pack.Status == PackageReceive.Success)
            {
                //扫描间隔改为2s，即扫描在2s内完成
                Dongle.SendCmd(PackageSend.GAP_SetParamPack(PackageSend.TGAP_GEN_DISC_SCAN, (UInt16)2000));
            }
            else
            {
                //重新发送初始化的命令
                Dongle.SendCmd(PackageSend.GAP_DeviceInitPack());
            }
        }
    }
}
