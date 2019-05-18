namespace Hyt.DataImporter
{
    partial class MyForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnhyt3 = new System.Windows.Forms.Button();
            this.btnhyt2_1 = new System.Windows.Forms.Button();
            this.txthyt3 = new System.Windows.Forms.TextBox();
            this.progressBarHYT3 = new System.Windows.Forms.ProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnSetAreaMapping = new System.Windows.Forms.Button();
            this.btnSetCategoryCode = new System.Windows.Forms.Button();
            this.progressBarHYT2_4 = new System.Windows.Forms.ProgressBar();
            this.btnhyt2_4 = new System.Windows.Forms.Button();
            this.progressBarHYT2_3 = new System.Windows.Forms.ProgressBar();
            this.btnhyt2_3 = new System.Windows.Forms.Button();
            this.progressBarHYT2_2 = new System.Windows.Forms.ProgressBar();
            this.progressBarHYT2_1 = new System.Windows.Forms.ProgressBar();
            this.btnhyt2_2 = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker4 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker5 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnhyt3
            // 
            this.btnhyt3.Location = new System.Drawing.Point(3, 0);
            this.btnhyt3.Name = "btnhyt3";
            this.btnhyt3.Size = new System.Drawing.Size(179, 23);
            this.btnhyt3.TabIndex = 1;
            this.btnhyt3.Text = "三期基础数据";
            this.btnhyt3.UseVisualStyleBackColor = true;
            this.btnhyt3.Click += new System.EventHandler(this.btnhyt3_Click);
            // 
            // btnhyt2_1
            // 
            this.btnhyt2_1.Location = new System.Drawing.Point(3, 82);
            this.btnhyt2_1.Name = "btnhyt2_1";
            this.btnhyt2_1.Size = new System.Drawing.Size(179, 23);
            this.btnhyt2_1.TabIndex = 2;
            this.btnhyt2_1.Text = "二期基础数据";
            this.btnhyt2_1.UseVisualStyleBackColor = true;
            this.btnhyt2_1.Click += new System.EventHandler(this.btnhyt2_1_Click);
            // 
            // txthyt3
            // 
            this.txthyt3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txthyt3.Location = new System.Drawing.Point(0, 0);
            this.txthyt3.Multiline = true;
            this.txthyt3.Name = "txthyt3";
            this.txthyt3.Size = new System.Drawing.Size(1039, 179);
            this.txthyt3.TabIndex = 3;
            this.txthyt3.TextChanged += new System.EventHandler(this.txthyt3_TextChanged);
            // 
            // progressBarHYT3
            // 
            this.progressBarHYT3.Location = new System.Drawing.Point(3, 32);
            this.progressBarHYT3.Name = "progressBarHYT3";
            this.progressBarHYT3.Size = new System.Drawing.Size(1029, 31);
            this.progressBarHYT3.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnSetAreaMapping);
            this.splitContainer1.Panel1.Controls.Add(this.btnSetCategoryCode);
            this.splitContainer1.Panel1.Controls.Add(this.progressBarHYT2_4);
            this.splitContainer1.Panel1.Controls.Add(this.progressBarHYT3);
            this.splitContainer1.Panel1.Controls.Add(this.btnhyt2_4);
            this.splitContainer1.Panel1.Controls.Add(this.btnhyt3);
            this.splitContainer1.Panel1.Controls.Add(this.progressBarHYT2_3);
            this.splitContainer1.Panel1.Controls.Add(this.btnhyt2_1);
            this.splitContainer1.Panel1.Controls.Add(this.btnhyt2_3);
            this.splitContainer1.Panel1.Controls.Add(this.progressBarHYT2_2);
            this.splitContainer1.Panel1.Controls.Add(this.progressBarHYT2_1);
            this.splitContainer1.Panel1.Controls.Add(this.btnhyt2_2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txthyt3);
            this.splitContainer1.Size = new System.Drawing.Size(1039, 613);
            this.splitContainer1.SplitterDistance = 430;
            this.splitContainer1.TabIndex = 5;
            // 
            // btnSetAreaMapping
            // 
            this.btnSetAreaMapping.Location = new System.Drawing.Point(0, 394);
            this.btnSetAreaMapping.Name = "btnSetAreaMapping";
            this.btnSetAreaMapping.Size = new System.Drawing.Size(182, 23);
            this.btnSetAreaMapping.TabIndex = 13;
            this.btnSetAreaMapping.Text = "设置区域映射表";
            this.btnSetAreaMapping.UseVisualStyleBackColor = true;
            this.btnSetAreaMapping.Click += new System.EventHandler(this.btnSetAreaMapping_Click);
            // 
            // btnSetCategoryCode
            // 
            this.btnSetCategoryCode.Location = new System.Drawing.Point(272, 394);
            this.btnSetCategoryCode.Name = "btnSetCategoryCode";
            this.btnSetCategoryCode.Size = new System.Drawing.Size(182, 23);
            this.btnSetCategoryCode.TabIndex = 12;
            this.btnSetCategoryCode.Text = "设置商品分类编码";
            this.btnSetCategoryCode.UseVisualStyleBackColor = true;
            this.btnSetCategoryCode.Visible = false;
            this.btnSetCategoryCode.Click += new System.EventHandler(this.btnSetCategoryCode_Click);
            // 
            // progressBarHYT2_4
            // 
            this.progressBarHYT2_4.Location = new System.Drawing.Point(0, 354);
            this.progressBarHYT2_4.Name = "progressBarHYT2_4";
            this.progressBarHYT2_4.Size = new System.Drawing.Size(1036, 34);
            this.progressBarHYT2_4.TabIndex = 11;
            // 
            // btnhyt2_4
            // 
            this.btnhyt2_4.Location = new System.Drawing.Point(3, 325);
            this.btnhyt2_4.Name = "btnhyt2_4";
            this.btnhyt2_4.Size = new System.Drawing.Size(182, 23);
            this.btnhyt2_4.TabIndex = 8;
            this.btnhyt2_4.Text = "二期业务数据(仓库物流)";
            this.btnhyt2_4.UseVisualStyleBackColor = true;
            this.btnhyt2_4.Click += new System.EventHandler(this.btnhyt2_4_Click);
            // 
            // progressBarHYT2_3
            // 
            this.progressBarHYT2_3.Location = new System.Drawing.Point(3, 271);
            this.progressBarHYT2_3.Name = "progressBarHYT2_3";
            this.progressBarHYT2_3.Size = new System.Drawing.Size(1032, 34);
            this.progressBarHYT2_3.TabIndex = 10;
            // 
            // btnhyt2_3
            // 
            this.btnhyt2_3.Location = new System.Drawing.Point(3, 242);
            this.btnhyt2_3.Name = "btnhyt2_3";
            this.btnhyt2_3.Size = new System.Drawing.Size(179, 23);
            this.btnhyt2_3.TabIndex = 7;
            this.btnhyt2_3.Text = "二期业务数据(订单)";
            this.btnhyt2_3.UseVisualStyleBackColor = true;
            this.btnhyt2_3.Click += new System.EventHandler(this.btnhyt2_3_Click);
            // 
            // progressBarHYT2_2
            // 
            this.progressBarHYT2_2.Location = new System.Drawing.Point(0, 189);
            this.progressBarHYT2_2.Name = "progressBarHYT2_2";
            this.progressBarHYT2_2.Size = new System.Drawing.Size(1035, 34);
            this.progressBarHYT2_2.TabIndex = 9;
            // 
            // progressBarHYT2_1
            // 
            this.progressBarHYT2_1.Location = new System.Drawing.Point(0, 111);
            this.progressBarHYT2_1.Name = "progressBarHYT2_1";
            this.progressBarHYT2_1.Size = new System.Drawing.Size(1032, 34);
            this.progressBarHYT2_1.TabIndex = 5;
            // 
            // btnhyt2_2
            // 
            this.btnhyt2_2.Location = new System.Drawing.Point(0, 160);
            this.btnhyt2_2.Name = "btnhyt2_2";
            this.btnhyt2_2.Size = new System.Drawing.Size(179, 23);
            this.btnhyt2_2.TabIndex = 6;
            this.btnhyt2_2.Text = "二期业务数据(财务)";
            this.btnhyt2_2.UseVisualStyleBackColor = true;
            this.btnhyt2_2.Click += new System.EventHandler(this.btnhyt2_2_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // backgroundWorker3
            // 
            this.backgroundWorker3.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker3_DoWork);
            this.backgroundWorker3.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker3_ProgressChanged);
            this.backgroundWorker3.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker3_RunWorkerCompleted);
            // 
            // backgroundWorker4
            // 
            this.backgroundWorker4.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker4_DoWork);
            this.backgroundWorker4.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker4_ProgressChanged);
            this.backgroundWorker4.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker4_RunWorkerCompleted);
            // 
            // backgroundWorker5
            // 
            this.backgroundWorker5.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker5_DoWork);
            this.backgroundWorker5.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker5_ProgressChanged);
            this.backgroundWorker5.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker5_RunWorkerCompleted);
            // 
            // MyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 613);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MyForm";
            this.Text = "MyForm";
            this.Load += new System.EventHandler(this.MyForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnhyt3;
        private System.Windows.Forms.Button btnhyt2_1;
        private System.Windows.Forms.TextBox txthyt3;
        private System.Windows.Forms.ProgressBar progressBarHYT3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ProgressBar progressBarHYT2_1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.Button btnhyt2_2;
        private System.Windows.Forms.Button btnhyt2_3;
        private System.Windows.Forms.Button btnhyt2_4;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.ComponentModel.BackgroundWorker backgroundWorker4;
        private System.ComponentModel.BackgroundWorker backgroundWorker5;
        private System.Windows.Forms.ProgressBar progressBarHYT2_4;
        private System.Windows.Forms.ProgressBar progressBarHYT2_3;
        private System.Windows.Forms.ProgressBar progressBarHYT2_2;
        private System.Windows.Forms.Button btnSetCategoryCode;
        private System.Windows.Forms.Button btnSetAreaMapping;
    }
}