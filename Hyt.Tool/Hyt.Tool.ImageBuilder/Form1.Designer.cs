namespace Hyt.Tool.ImageBuilder
{
    partial class Form1
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
            this.status = new System.Windows.Forms.StatusStrip();
            this.pbProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.lbProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.cb460 = new System.Windows.Forms.CheckBox();
            this.cb240 = new System.Windows.Forms.CheckBox();
            this.cb200 = new System.Windows.Forms.CheckBox();
            this.cb180 = new System.Windows.Forms.CheckBox();
            this.cb120 = new System.Windows.Forms.CheckBox();
            this.cb100 = new System.Windows.Forms.CheckBox();
            this.cb80 = new System.Windows.Forms.CheckBox();
            this.cb60 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSourceFolder = new System.Windows.Forms.TextBox();
            this.tbTargetFolder = new System.Windows.Forms.TextBox();
            this.btnSource = new System.Windows.Forms.Button();
            this.btnTarget = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.cbOther = new System.Windows.Forms.CheckBox();
            this.mtbO1 = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblO1 = new System.Windows.Forms.Label();
            this.btnThumbnail = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.status.SuspendLayout();
            this.SuspendLayout();
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbProgress,
            this.lbProgress,
            this.toolStripStatusLabel1});
            this.status.Location = new System.Drawing.Point(0, 197);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(669, 22);
            this.status.TabIndex = 0;
            this.status.Text = "statusStrip1";
            // 
            // pbProgress
            // 
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(100, 16);
            this.pbProgress.Visible = false;
            // 
            // lbProgress
            // 
            this.lbProgress.Name = "lbProgress";
            this.lbProgress.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // cb460
            // 
            this.cb460.AutoSize = true;
            this.cb460.Checked = true;
            this.cb460.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb460.Location = new System.Drawing.Point(26, 101);
            this.cb460.Name = "cb460";
            this.cb460.Size = new System.Drawing.Size(72, 16);
            this.cb460.TabIndex = 1;
            this.cb460.Text = "460×460";
            this.cb460.UseVisualStyleBackColor = true;
            // 
            // cb240
            // 
            this.cb240.AutoSize = true;
            this.cb240.Checked = true;
            this.cb240.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb240.Location = new System.Drawing.Point(108, 101);
            this.cb240.Name = "cb240";
            this.cb240.Size = new System.Drawing.Size(72, 16);
            this.cb240.TabIndex = 1;
            this.cb240.Text = "240×240";
            this.cb240.UseVisualStyleBackColor = true;
            // 
            // cb200
            // 
            this.cb200.AutoSize = true;
            this.cb200.Checked = true;
            this.cb200.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb200.Location = new System.Drawing.Point(186, 101);
            this.cb200.Name = "cb200";
            this.cb200.Size = new System.Drawing.Size(72, 16);
            this.cb200.TabIndex = 1;
            this.cb200.Text = "200×200";
            this.cb200.UseVisualStyleBackColor = true;
            // 
            // cb180
            // 
            this.cb180.AutoSize = true;
            this.cb180.Checked = true;
            this.cb180.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb180.Location = new System.Drawing.Point(264, 101);
            this.cb180.Name = "cb180";
            this.cb180.Size = new System.Drawing.Size(72, 16);
            this.cb180.TabIndex = 1;
            this.cb180.Text = "180×180";
            this.cb180.UseVisualStyleBackColor = true;
            // 
            // cb120
            // 
            this.cb120.AutoSize = true;
            this.cb120.Checked = true;
            this.cb120.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb120.Location = new System.Drawing.Point(342, 101);
            this.cb120.Name = "cb120";
            this.cb120.Size = new System.Drawing.Size(72, 16);
            this.cb120.TabIndex = 1;
            this.cb120.Text = "120×120";
            this.cb120.UseVisualStyleBackColor = true;
            // 
            // cb100
            // 
            this.cb100.AutoSize = true;
            this.cb100.Checked = true;
            this.cb100.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb100.Location = new System.Drawing.Point(420, 101);
            this.cb100.Name = "cb100";
            this.cb100.Size = new System.Drawing.Size(72, 16);
            this.cb100.TabIndex = 1;
            this.cb100.Text = "100×100";
            this.cb100.UseVisualStyleBackColor = true;
            // 
            // cb80
            // 
            this.cb80.AutoSize = true;
            this.cb80.Checked = true;
            this.cb80.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb80.Location = new System.Drawing.Point(498, 101);
            this.cb80.Name = "cb80";
            this.cb80.Size = new System.Drawing.Size(60, 16);
            this.cb80.TabIndex = 1;
            this.cb80.Text = "80×80";
            this.cb80.UseVisualStyleBackColor = true;
            // 
            // cb60
            // 
            this.cb60.AutoSize = true;
            this.cb60.Checked = true;
            this.cb60.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb60.Location = new System.Drawing.Point(564, 101);
            this.cb60.Name = "cb60";
            this.cb60.Size = new System.Drawing.Size(60, 16);
            this.cb60.TabIndex = 1;
            this.cb60.Text = "60×60";
            this.cb60.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "原始图片路径";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "目标图片路径";
            // 
            // tbSourceFolder
            // 
            this.tbSourceFolder.Location = new System.Drawing.Point(108, 18);
            this.tbSourceFolder.Name = "tbSourceFolder";
            this.tbSourceFolder.Size = new System.Drawing.Size(442, 21);
            this.tbSourceFolder.TabIndex = 3;
            // 
            // tbTargetFolder
            // 
            this.tbTargetFolder.Location = new System.Drawing.Point(108, 54);
            this.tbTargetFolder.Name = "tbTargetFolder";
            this.tbTargetFolder.Size = new System.Drawing.Size(442, 21);
            this.tbTargetFolder.TabIndex = 3;
            // 
            // btnSource
            // 
            this.btnSource.Location = new System.Drawing.Point(564, 17);
            this.btnSource.Name = "btnSource";
            this.btnSource.Size = new System.Drawing.Size(75, 23);
            this.btnSource.TabIndex = 4;
            this.btnSource.Text = "选择...";
            this.btnSource.UseVisualStyleBackColor = true;
            this.btnSource.Click += new System.EventHandler(this.btnSource_Click);
            // 
            // btnTarget
            // 
            this.btnTarget.Location = new System.Drawing.Point(564, 53);
            this.btnTarget.Name = "btnTarget";
            this.btnTarget.Size = new System.Drawing.Size(75, 23);
            this.btnTarget.TabIndex = 4;
            this.btnTarget.Text = "选择...";
            this.btnTarget.UseVisualStyleBackColor = true;
            this.btnTarget.Click += new System.EventHandler(this.btnTarget_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(23, 160);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(100, 23);
            this.btnConvert.TabIndex = 5;
            this.btnConvert.Text = "开始转换比例图";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // cbOther
            // 
            this.cbOther.AutoSize = true;
            this.cbOther.Location = new System.Drawing.Point(26, 128);
            this.cbOther.Name = "cbOther";
            this.cbOther.Size = new System.Drawing.Size(48, 16);
            this.cbOther.TabIndex = 6;
            this.cbOther.Text = "其他";
            this.cbOther.UseVisualStyleBackColor = true;
            // 
            // mtbO1
            // 
            this.mtbO1.Location = new System.Drawing.Point(71, 126);
            this.mtbO1.Mask = "9999";
            this.mtbO1.Name = "mtbO1";
            this.mtbO1.Size = new System.Drawing.Size(33, 21);
            this.mtbO1.TabIndex = 7;
            this.mtbO1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mtbO1_KeyUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(106, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "×";
            // 
            // lblO1
            // 
            this.lblO1.AutoSize = true;
            this.lblO1.Location = new System.Drawing.Point(123, 129);
            this.lblO1.Name = "lblO1";
            this.lblO1.Size = new System.Drawing.Size(11, 12);
            this.lblO1.TabIndex = 10;
            this.lblO1.Text = "0";
            // 
            // btnThumbnail
            // 
            this.btnThumbnail.Location = new System.Drawing.Point(141, 160);
            this.btnThumbnail.Name = "btnThumbnail";
            this.btnThumbnail.Size = new System.Drawing.Size(100, 23);
            this.btnThumbnail.TabIndex = 11;
            this.btnThumbnail.Text = "开始转换缩略图";
            this.btnThumbnail.UseVisualStyleBackColor = true;
            this.btnThumbnail.Click += new System.EventHandler(this.btnThumbnail_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(272, 160);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "开始转换产品缩略图";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 219);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnThumbnail);
            this.Controls.Add(this.lblO1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mtbO1);
            this.Controls.Add(this.cbOther);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.btnTarget);
            this.Controls.Add(this.btnSource);
            this.Controls.Add(this.tbTargetFolder);
            this.Controls.Add(this.tbSourceFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb60);
            this.Controls.Add(this.cb80);
            this.Controls.Add(this.cb100);
            this.Controls.Add(this.cb120);
            this.Controls.Add(this.cb180);
            this.Controls.Add(this.cb200);
            this.Controls.Add(this.cb240);
            this.Controls.Add(this.cb460);
            this.Controls.Add(this.status);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图片转换工具";
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripProgressBar pbProgress;
        private System.Windows.Forms.ToolStripStatusLabel lbProgress;
        private System.Windows.Forms.CheckBox cb460;
        private System.Windows.Forms.CheckBox cb240;
        private System.Windows.Forms.CheckBox cb200;
        private System.Windows.Forms.CheckBox cb180;
        private System.Windows.Forms.CheckBox cb120;
        private System.Windows.Forms.CheckBox cb100;
        private System.Windows.Forms.CheckBox cb80;
        private System.Windows.Forms.CheckBox cb60;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSourceFolder;
        private System.Windows.Forms.TextBox tbTargetFolder;
        private System.Windows.Forms.Button btnSource;
        private System.Windows.Forms.Button btnTarget;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.CheckBox cbOther;
        private System.Windows.Forms.MaskedTextBox mtbO1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblO1;
        private System.Windows.Forms.Button btnThumbnail;
        private System.Windows.Forms.Button button1;
    }
}

