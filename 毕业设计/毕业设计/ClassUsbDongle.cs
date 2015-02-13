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

        /// <summary>
        /// 初始化，设定好定时器和订阅好事件
        /// </summary>
        public UsbDongle()
        {
            comm = new SerialPort();
            comm.NewLine = "\r\n";
            comm.RtsEnable = true;//视个人情况

            TimerCheck.Enabled = true;
            TimerCheck.Interval = 4000;//设定定时器,3秒一次
            TimerCheck.Stop();
            TimerCheck.Tick +=TimerCheck_Tick;

            this.GAP_DeviceInitDoneEventPackComeEvent += UsbDongle_GAP_DeviceInitDoneEventPackComeEvent;
        }

        /// <summary>
        /// 串口收到数据后，把数据包（串口包）存到一个队列里面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void comm_DataReceived(object sender, SerialDataReceivedEventArgs e)//串口收到数据后的回调函数
        {
            SerialPort Port = (SerialPort)sender;
            int n = Port.BytesToRead;
            byte[] buf = new byte[n];
            Port.Read(buf, 0, n);//读取串口信息到buf里面
            PackQue.Enqueue(buf);//入队列
        }
        
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="text"></param>
        public void DongleOpen(string text)   //打开串口
        {
            comm.PortName = text;
            comm.BaudRate = 115200;
            try
            {
                comm.Open();//打开串口后马上初始化USB Dongle
                comm.DiscardInBuffer();
                comm.DiscardOutBuffer();
                comm.DataReceived += new SerialDataReceivedEventHandler(comm_DataReceived); //串口接收到数据的回调函数
            }
            catch (Exception ex)
            {
                comm = new SerialPort();
                TimerCheck.Stop();
                MessageBox.Show(ex.Message);
            }
            if (comm.IsOpen) TimerCheck.Start();//打开了才开始定时器
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void DongleClose()       //关闭串口
        {
            comm.Close();
            TimerCheck.Stop();
        }

        /// <summary>
        /// 处理队列中的串口包，根据包的类型触发不同类型的事件
        /// </summary>
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
                       //
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

        /// <summary>
        /// 发送数据，简单的串口写
        /// </summary>
        /// <param name="SendData"></param>
        public void SendCmd(byte[] SendData)    //发送命令
        {
            comm.Write(SendData, 0, SendData.Length);
        }

        /// <summary>
        /// 定时扫描和处理队列里面的串口包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimerCheck_Tick(object sender, EventArgs e)    //3秒一次
        {
            PackageProcess();       //先处理完所有的包，再发出扫描的信号
            this.SendCmd(PackageSend.GAP_DeviceDiscoveryRequestPack());
        }

        /// <summary>
        /// 收到初始化完成的包后，把扫描时间调好
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void UsbDongle_GAP_DeviceInitDoneEventPackComeEvent(object sender, PackEventArgs e)//收到初始化完成的包的处理函数
        {
            GAP_DeviceInitDonePack Pack = new GAP_DeviceInitDonePack(e._pack);
            UsbDongle Dongle = (UsbDongle)sender;
            if (Pack.Status == PackageReceive.Success)
            {
                //扫描间隔改为2s，即扫描在2s内完成
                Dongle.SendCmd(PackageSend.GAP_SetParamPack(PackageSend.TGAP_GEN_DISC_SCAN, (UInt16)3000));
            }
            else
            {
                //重新发送初始化的命令
                Dongle.SendCmd(PackageSend.GAP_DeviceInitPack());
            }
        }
    }
}
