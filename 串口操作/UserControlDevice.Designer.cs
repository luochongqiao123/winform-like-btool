namespace 串口操作
{
    partial class UserControlDevice
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelShowTemp = new System.Windows.Forms.Label();
            this.labelShowHumi = new System.Windows.Forms.Label();
            this.labelTemp = new System.Windows.Forms.Label();
            this.labelHumi = new System.Windows.Forms.Label();
            this.labelShowAddr = new System.Windows.Forms.Label();
            this.labelShowName = new System.Windows.Forms.Label();
            this.labelShowRssi = new System.Windows.Forms.Label();
            this.labelDeviceAddr = new System.Windows.Forms.Label();
            this.labelDeviceName = new System.Windows.Forms.Label();
            this.labelRssi = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelShowTemp
            // 
            this.labelShowTemp.AutoSize = true;
            this.labelShowTemp.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelShowTemp.Location = new System.Drawing.Point(3, 10);
            this.labelShowTemp.Name = "labelShowTemp";
            this.labelShowTemp.Size = new System.Drawing.Size(49, 20);
            this.labelShowTemp.TabIndex = 0;
            this.labelShowTemp.Text = "Temp";
            // 
            // labelShowHumi
            // 
            this.labelShowHumi.AutoSize = true;
            this.labelShowHumi.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelShowHumi.Location = new System.Drawing.Point(98, 10);
            this.labelShowHumi.Name = "labelShowHumi";
            this.labelShowHumi.Size = new System.Drawing.Size(49, 20);
            this.labelShowHumi.TabIndex = 1;
            this.labelShowHumi.Text = "Humi";
            // 
            // labelTemp
            // 
            this.labelTemp.AutoSize = true;
            this.labelTemp.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTemp.Location = new System.Drawing.Point(28, 30);
            this.labelTemp.Name = "labelTemp";
            this.labelTemp.Size = new System.Drawing.Size(24, 16);
            this.labelTemp.TabIndex = 2;
            this.labelTemp.Text = "21";
            // 
            // labelHumi
            // 
            this.labelHumi.AutoSize = true;
            this.labelHumi.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelHumi.Location = new System.Drawing.Point(123, 30);
            this.labelHumi.Name = "labelHumi";
            this.labelHumi.Size = new System.Drawing.Size(24, 16);
            this.labelHumi.TabIndex = 3;
            this.labelHumi.Text = "80";
            // 
            // labelShowAddr
            // 
            this.labelShowAddr.AutoSize = true;
            this.labelShowAddr.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelShowAddr.Location = new System.Drawing.Point(5, 55);
            this.labelShowAddr.Name = "labelShowAddr";
            this.labelShowAddr.Size = new System.Drawing.Size(84, 14);
            this.labelShowAddr.TabIndex = 4;
            this.labelShowAddr.Text = "DeviceAddr:";
            // 
            // labelShowName
            // 
            this.labelShowName.AutoSize = true;
            this.labelShowName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelShowName.Location = new System.Drawing.Point(5, 81);
            this.labelShowName.Name = "labelShowName";
            this.labelShowName.Size = new System.Drawing.Size(84, 14);
            this.labelShowName.TabIndex = 5;
            this.labelShowName.Text = "DeviceName:";
            // 
            // labelShowRssi
            // 
            this.labelShowRssi.AutoSize = true;
            this.labelShowRssi.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelShowRssi.Location = new System.Drawing.Point(5, 107);
            this.labelShowRssi.Name = "labelShowRssi";
            this.labelShowRssi.Size = new System.Drawing.Size(35, 14);
            this.labelShowRssi.TabIndex = 6;
            this.labelShowRssi.Text = "Rssi";
            // 
            // labelDeviceAddr
            // 
            this.labelDeviceAddr.AutoSize = true;
            this.labelDeviceAddr.Location = new System.Drawing.Point(29, 69);
            this.labelDeviceAddr.Name = "labelDeviceAddr";
            this.labelDeviceAddr.Size = new System.Drawing.Size(107, 12);
            this.labelDeviceAddr.TabIndex = 7;
            this.labelDeviceAddr.Text = "00:00:00:00:00:00";
            // 
            // labelDeviceName
            // 
            this.labelDeviceName.AutoSize = true;
            this.labelDeviceName.Location = new System.Drawing.Point(29, 95);
            this.labelDeviceName.Name = "labelDeviceName";
            this.labelDeviceName.Size = new System.Drawing.Size(101, 12);
            this.labelDeviceName.TabIndex = 8;
            this.labelDeviceName.Text = "SimpleprofileBLE";
            // 
            // labelRssi
            // 
            this.labelRssi.AutoSize = true;
            this.labelRssi.Location = new System.Drawing.Point(29, 121);
            this.labelRssi.Name = "labelRssi";
            this.labelRssi.Size = new System.Drawing.Size(23, 12);
            this.labelRssi.TabIndex = 9;
            this.labelRssi.Text = "210";
            // 
            // UserControlDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelRssi);
            this.Controls.Add(this.labelDeviceName);
            this.Controls.Add(this.labelDeviceAddr);
            this.Controls.Add(this.labelShowRssi);
            this.Controls.Add(this.labelShowName);
            this.Controls.Add(this.labelShowAddr);
            this.Controls.Add(this.labelHumi);
            this.Controls.Add(this.labelTemp);
            this.Controls.Add(this.labelShowHumi);
            this.Controls.Add(this.labelShowTemp);
            this.Name = "UserControlDevice";
            this.Size = new System.Drawing.Size(159, 154);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelShowTemp;
        private System.Windows.Forms.Label labelShowHumi;
        private System.Windows.Forms.Label labelTemp;
        private System.Windows.Forms.Label labelHumi;
        private System.Windows.Forms.Label labelShowAddr;
        private System.Windows.Forms.Label labelShowName;
        private System.Windows.Forms.Label labelShowRssi;
        private System.Windows.Forms.Label labelDeviceAddr;
        private System.Windows.Forms.Label labelDeviceName;
        private System.Windows.Forms.Label labelRssi;
    }
}
