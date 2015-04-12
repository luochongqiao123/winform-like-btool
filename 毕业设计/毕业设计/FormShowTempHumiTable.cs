using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NPlot;

namespace 毕业设计
{
    public partial class FormShowTempHumiTable : Form
    {
        public FormShowTempHumiTable()
        {
            InitializeComponent();
        }

        public FormShowTempHumiTable(string FormNameText, ToolStripMenuItem OnOff )
        {
            InitializeComponent();
            this.Text = FormNameText;
            this.ItemOnOff = OnOff;
        }

        //用于控制右键菜单的使能和失能
        ToolStripMenuItem ItemOnOff;

        private void FormShowTempHumiTable_Load(object sender, EventArgs e)
        {
            //plotline();
        }

        public void plotline()
        {
            // --- Plotting ---
            plotSurface2D_tempHumi.Clear();
            //plotSurface2D_tempHumi.
            // --- Grid Code ---
            Grid mygrid = new Grid();
            mygrid.HorizontalGridType = Grid.GridType.Fine;
            mygrid.VerticalGridType = Grid.GridType.Fine;
            
            plotSurface2D_tempHumi.Add(mygrid);
            plotSurface2D_tempHumi.Title = "Test";


            LinePlot lp3 = new LinePlot();
            lp3.AbscissaData = new int[] { 1, 2, 3, 4, 5, 6 };
            //lp3.OrdinateData = new int[] { 1, 1, 1, 1, 2, 1 };
            Queue<double> qiao = new Queue<double>();
            qiao.Enqueue(2.3);
            qiao.Enqueue(2.4);
            qiao.Enqueue(2.5);
            qiao.Enqueue(2.6);
            lp3.OrdinateData = qiao;
            lp3.Pen = new Pen(Color.Orange,2);
            //lp3.Pen.Width = 2;
            lp3.Label = " 价格";
            this.plotSurface2D_tempHumi.Add(lp3);

            LinearAxis liny1 = (LinearAxis)plotSurface2D_tempHumi.YAxis1;
            liny1.Label = "价格";
            liny1.AxisColor = Color.Orange;
            liny1.LabelColor = Color.Orange;
            liny1.TickTextColor = Color.Orange;

           // plotSurface2D_tempHumi.Refresh();

            lp3.AbscissaData = new int[] { 1, 2, 3, 4, 5, 6 };
            lp3.OrdinateData = new int[] { 1,3, 1, 4, 2, 1 };
            
           // plotSurface2D_tempHumi.Remove(lp3,true);
            plotSurface2D_tempHumi.Refresh();
            return;
        }

        //更新弹窗上的曲线图
        public void DrawNewData(double[] Tempx, double[] Tempy, double[] Humix, double[] Humiy)
        {
            //先清空
            plotSurface2D_tempHumi.Clear();
            //画坐标
            Grid TempHumiGrid = new Grid();
            TempHumiGrid.HorizontalGridType = Grid.GridType.Fine;
            TempHumiGrid.VerticalGridType = Grid.GridType.Fine;
            plotSurface2D_tempHumi.Add(TempHumiGrid);
            //标题
            plotSurface2D_tempHumi.Title = "温湿度测控";
            //温度线
            LinePlot LineTemp = new LinePlot(Tempx, Tempy);
            LineTemp.Pen = new Pen(Color.Red, 3);
            this.plotSurface2D_tempHumi.Add(LineTemp);
            //刷新显示
            plotSurface2D_tempHumi.Refresh();

        }

        /// <summary>
        /// a关闭窗口时候把右键按钮使能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormShowTempHumiTable_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ItemOnOff.Enabled = true;
        }
    }
}
