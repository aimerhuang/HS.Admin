namespace Hyt.DataImporter
{
    partial class FrmMain
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
            this.BtnBeginImport = new System.Windows.Forms.Button();
            this.TxtMessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BtnBeginImport
            // 
            this.BtnBeginImport.Location = new System.Drawing.Point(46, 281);
            this.BtnBeginImport.Name = "BtnBeginImport";
            this.BtnBeginImport.Size = new System.Drawing.Size(75, 23);
            this.BtnBeginImport.TabIndex = 0;
            this.BtnBeginImport.Text = "开始导入";
            this.BtnBeginImport.UseVisualStyleBackColor = true;
            this.BtnBeginImport.Click += new System.EventHandler(this.BtnBeginImport_Click);
            // 
            // TxtMessage
            // 
            this.TxtMessage.Location = new System.Drawing.Point(46, 42);
            this.TxtMessage.Multiline = true;
            this.TxtMessage.Name = "TxtMessage";
            this.TxtMessage.Size = new System.Drawing.Size(490, 208);
            this.TxtMessage.TabIndex = 1;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.TxtMessage);
            this.Controls.Add(this.BtnBeginImport);
            this.Name = "FrmMain";
            this.Text = "数据导入工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnBeginImport;
        private System.Windows.Forms.TextBox TxtMessage;

    }
}

