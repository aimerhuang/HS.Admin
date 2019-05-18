namespace Hyt.SMS
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
            this.webService1 = new Extra.SMS.mandao.WebService();
            this.tbxMobiles = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxMessage = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webService1
            // 
            this.webService1.Credentials = null;
            this.webService1.Url = "http://sdk3.entinfo.cn:8060/webservice.asmx";
            this.webService1.UseDefaultCredentials = false;
            // 
            // tbxMobiles
            // 
            this.tbxMobiles.Location = new System.Drawing.Point(12, 19);
            this.tbxMobiles.Multiline = true;
            this.tbxMobiles.Name = "tbxMobiles";
            this.tbxMobiles.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxMobiles.Size = new System.Drawing.Size(214, 497);
            this.tbxMobiles.TabIndex = 0;
            this.tbxMobiles.Text = "13981812058";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "手机号码:";
            // 
            // tbxMessage
            // 
            this.tbxMessage.Location = new System.Drawing.Point(250, 19);
            this.tbxMessage.Multiline = true;
            this.tbxMessage.Name = "tbxMessage";
            this.tbxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxMessage.Size = new System.Drawing.Size(529, 238);
            this.tbxMessage.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(250, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "消息内容:";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("微软雅黑", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(250, 264);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(529, 223);
            this.button1.TabIndex = 4;
            this.button1.Text = "发送";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 518);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxMessage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxMobiles);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Extra.SMS.mandao.WebService webService1;
        private System.Windows.Forms.TextBox tbxMobiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxMessage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}

