﻿namespace 毕业设计
{
    partial class FormTempHumi
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
            this.comboBoxPort = new System.Windows.Forms.ComboBox();
            this.buttonOpenClose = new System.Windows.Forms.Button();
            this.flowLayoutPanelMain = new System.Windows.Forms.FlowLayoutPanel();
            this.labelShowStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxPort
            // 
            this.comboBoxPort.FormattingEnabled = true;
            this.comboBoxPort.Location = new System.Drawing.Point(12, 12);
            this.comboBoxPort.Name = "comboBoxPort";
            this.comboBoxPort.Size = new System.Drawing.Size(121, 20);
            this.comboBoxPort.TabIndex = 0;
            // 
            // buttonOpenClose
            // 
            this.buttonOpenClose.Location = new System.Drawing.Point(139, 12);
            this.buttonOpenClose.Name = "buttonOpenClose";
            this.buttonOpenClose.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenClose.TabIndex = 1;
            this.buttonOpenClose.Text = "Open";
            this.buttonOpenClose.UseVisualStyleBackColor = true;
            this.buttonOpenClose.Click += new System.EventHandler(this.buttonOpenClose_Click);
            // 
            // flowLayoutPanelMain
            // 
            this.flowLayoutPanelMain.Location = new System.Drawing.Point(12, 41);
            this.flowLayoutPanelMain.Name = "flowLayoutPanelMain";
            this.flowLayoutPanelMain.Size = new System.Drawing.Size(606, 322);
            this.flowLayoutPanelMain.TabIndex = 2;
            // 
            // labelShowStatus
            // 
            this.labelShowStatus.AutoSize = true;
            this.labelShowStatus.Font = new System.Drawing.Font("Trebuchet MS", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShowStatus.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.labelShowStatus.Location = new System.Drawing.Point(261, 16);
            this.labelShowStatus.Name = "labelShowStatus";
            this.labelShowStatus.Size = new System.Drawing.Size(12, 18);
            this.labelShowStatus.TabIndex = 3;
            this.labelShowStatus.Text = " ";
            // 
            // FormTempHumi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 398);
            this.Controls.Add(this.labelShowStatus);
            this.Controls.Add(this.flowLayoutPanelMain);
            this.Controls.Add(this.buttonOpenClose);
            this.Controls.Add(this.comboBoxPort);
            this.Name = "FormTempHumi";
            this.Text = "上位机";
            this.Load += new System.EventHandler(this.FormTempHumi_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxPort;
        private System.Windows.Forms.Button buttonOpenClose;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelMain;
        private System.Windows.Forms.Label labelShowStatus;
    }
}

