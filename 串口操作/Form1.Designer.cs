namespace 串口操作
{
    partial class FormPort
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBoxPortName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOpenClose = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.textBoxGet = new System.Windows.Forms.TextBox();
            this.textBoxSend = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.timerCheak = new System.Windows.Forms.Timer(this.components);
            this.textBoxAdvertData = new System.Windows.Forms.TextBox();
            this.labelQueueCount = new System.Windows.Forms.Label();
            this.userControlDevice1 = new 串口操作.UserControlDevice();
            this.SuspendLayout();
            // 
            // comboBoxPortName
            // 
            this.comboBoxPortName.FormattingEnabled = true;
            this.comboBoxPortName.Location = new System.Drawing.Point(111, 37);
            this.comboBoxPortName.Name = "comboBoxPortName";
            this.comboBoxPortName.Size = new System.Drawing.Size(121, 20);
            this.comboBoxPortName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(30, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "PortName";
            // 
            // buttonOpenClose
            // 
            this.buttonOpenClose.Location = new System.Drawing.Point(330, 28);
            this.buttonOpenClose.Name = "buttonOpenClose";
            this.buttonOpenClose.Size = new System.Drawing.Size(84, 29);
            this.buttonOpenClose.TabIndex = 2;
            this.buttonOpenClose.Text = "open";
            this.buttonOpenClose.UseVisualStyleBackColor = true;
            this.buttonOpenClose.Click += new System.EventHandler(this.buttonOpenClose_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(445, 28);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(84, 29);
            this.buttonReset.TabIndex = 3;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // textBoxGet
            // 
            this.textBoxGet.Location = new System.Drawing.Point(33, 81);
            this.textBoxGet.Multiline = true;
            this.textBoxGet.Name = "textBoxGet";
            this.textBoxGet.Size = new System.Drawing.Size(496, 173);
            this.textBoxGet.TabIndex = 4;
            // 
            // textBoxSend
            // 
            this.textBoxSend.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxSend.Location = new System.Drawing.Point(33, 297);
            this.textBoxSend.Name = "textBoxSend";
            this.textBoxSend.Size = new System.Drawing.Size(406, 23);
            this.textBoxSend.TabIndex = 5;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(475, 286);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(82, 34);
            this.buttonSend.TabIndex = 6;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // timerCheak
            // 
            this.timerCheak.Enabled = true;
            this.timerCheak.Interval = 1000;
            this.timerCheak.Tick += new System.EventHandler(this.timerCheak_Tick);
            // 
            // textBoxAdvertData
            // 
            this.textBoxAdvertData.Location = new System.Drawing.Point(546, 81);
            this.textBoxAdvertData.Multiline = true;
            this.textBoxAdvertData.Name = "textBoxAdvertData";
            this.textBoxAdvertData.Size = new System.Drawing.Size(223, 173);
            this.textBoxAdvertData.TabIndex = 7;
            // 
            // labelQueueCount
            // 
            this.labelQueueCount.AutoSize = true;
            this.labelQueueCount.Location = new System.Drawing.Point(610, 40);
            this.labelQueueCount.Name = "labelQueueCount";
            this.labelQueueCount.Size = new System.Drawing.Size(41, 12);
            this.labelQueueCount.TabIndex = 8;
            this.labelQueueCount.Text = "label2";
            // 
            // userControlDevice1
            // 
            this.userControlDevice1.Location = new System.Drawing.Point(33, 326);
            this.userControlDevice1.Name = "userControlDevice1";
            this.userControlDevice1.Size = new System.Drawing.Size(159, 154);
            this.userControlDevice1.TabIndex = 9;
            // 
            // FormPort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 479);
            this.Controls.Add(this.userControlDevice1);
            this.Controls.Add(this.labelQueueCount);
            this.Controls.Add(this.textBoxAdvertData);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textBoxSend);
            this.Controls.Add(this.textBoxGet);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonOpenClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxPortName);
            this.Name = "FormPort";
            this.Text = "串口操作";
            this.Load += new System.EventHandler(this.FormPort_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxPortName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOpenClose;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.TextBox textBoxGet;
        private System.Windows.Forms.TextBox textBoxSend;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Timer timerCheak;
        private System.Windows.Forms.TextBox textBoxAdvertData;
        private System.Windows.Forms.Label labelQueueCount;
        private UserControlDevice userControlDevice1;
    }
}

