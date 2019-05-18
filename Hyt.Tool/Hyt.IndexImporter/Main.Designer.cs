namespace Hyt.IndexImporter
{
    partial class Main
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lbProgress = new System.Windows.Forms.Label();
            this.bnCreateIndex = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(10, 21);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(262, 25);
            this.progressBar1.TabIndex = 0;
            // 
            // lbProgress
            // 
            this.lbProgress.AutoSize = true;
            this.lbProgress.Location = new System.Drawing.Point(278, 29);
            this.lbProgress.Name = "lbProgress";
            this.lbProgress.Size = new System.Drawing.Size(17, 12);
            this.lbProgress.TabIndex = 1;
            this.lbProgress.Text = "0%";
            // 
            // bnCreateIndex
            // 
            this.bnCreateIndex.Location = new System.Drawing.Point(104, 67);
            this.bnCreateIndex.Name = "bnCreateIndex";
            this.bnCreateIndex.Size = new System.Drawing.Size(89, 23);
            this.bnCreateIndex.TabIndex = 2;
            this.bnCreateIndex.Text = "创建商品索引";
            this.bnCreateIndex.UseVisualStyleBackColor = true;
            this.bnCreateIndex.Click += new System.EventHandler(this.bnCreateIndex_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 133);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(309, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 155);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.bnCreateIndex);
            this.Controls.Add(this.lbProgress);
            this.Controls.Add(this.progressBar1);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "生产索引";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lbProgress;
        private System.Windows.Forms.Button bnCreateIndex;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

