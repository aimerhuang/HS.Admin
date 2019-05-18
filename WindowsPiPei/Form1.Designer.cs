namespace WindowsPiPei
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
            this.btnOutToExcel = new System.Windows.Forms.Button();
            this.btnCompare = new System.Windows.Forms.Button();
            this.dgvCompareData = new System.Windows.Forms.DataGridView();
            this.FNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FBarcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labCountTitle = new System.Windows.Forms.Label();
            this.labCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompareData)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOutToExcel
            // 
            this.btnOutToExcel.Location = new System.Drawing.Point(1068, 53);
            this.btnOutToExcel.Name = "btnOutToExcel";
            this.btnOutToExcel.Size = new System.Drawing.Size(75, 23);
            this.btnOutToExcel.TabIndex = 1;
            this.btnOutToExcel.Text = "导出";
            this.btnOutToExcel.UseVisualStyleBackColor = true;
            this.btnOutToExcel.Click += new System.EventHandler(this.btnOutToExcel_Click);
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(608, 53);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(75, 23);
            this.btnCompare.TabIndex = 2;
            this.btnCompare.Text = "对比";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // dgvCompareData
            // 
            this.dgvCompareData.AllowUserToAddRows = false;
            this.dgvCompareData.AllowUserToDeleteRows = false;
            this.dgvCompareData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCompareData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FNumber,
            this.FBarcode,
            this.FFullName,
            this.FName});
            this.dgvCompareData.Location = new System.Drawing.Point(12, 82);
            this.dgvCompareData.Name = "dgvCompareData";
            this.dgvCompareData.ReadOnly = true;
            this.dgvCompareData.RowTemplate.Height = 23;
            this.dgvCompareData.Size = new System.Drawing.Size(1212, 353);
            this.dgvCompareData.TabIndex = 3;
            this.dgvCompareData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompareData_CellContentClick);
            // 
            // FNumber
            // 
            this.FNumber.DataPropertyName = "FNumber";
            this.FNumber.HeaderText = "商品编码";
            this.FNumber.Name = "FNumber";
            this.FNumber.ReadOnly = true;
            this.FNumber.Width = 150;
            // 
            // FBarcode
            // 
            this.FBarcode.DataPropertyName = "FBarcode";
            this.FBarcode.HeaderText = "条形码";
            this.FBarcode.Name = "FBarcode";
            this.FBarcode.ReadOnly = true;
            this.FBarcode.Width = 150;
            // 
            // FFullName
            // 
            this.FFullName.DataPropertyName = "FFullName";
            this.FFullName.HeaderText = "商品全名";
            this.FFullName.Name = "FFullName";
            this.FFullName.ReadOnly = true;
            this.FFullName.Width = 470;
            // 
            // FName
            // 
            this.FName.DataPropertyName = "FName";
            this.FName.HeaderText = "商品名称";
            this.FName.Name = "FName";
            this.FName.ReadOnly = true;
            this.FName.Width = 400;
            // 
            // labCountTitle
            // 
            this.labCountTitle.AutoSize = true;
            this.labCountTitle.Location = new System.Drawing.Point(376, 53);
            this.labCountTitle.Name = "labCountTitle";
            this.labCountTitle.Size = new System.Drawing.Size(47, 12);
            this.labCountTitle.TabIndex = 4;
            this.labCountTitle.Text = "总条数:";
            // 
            // labCount
            // 
            this.labCount.AutoSize = true;
            this.labCount.Location = new System.Drawing.Point(429, 53);
            this.labCount.Name = "labCount";
            this.labCount.Size = new System.Drawing.Size(41, 12);
            this.labCount.TabIndex = 5;
            this.labCount.Text = "label1";
            this.labCount.Click += new System.EventHandler(this.labCount_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1238, 447);
            this.Controls.Add(this.labCount);
            this.Controls.Add(this.labCountTitle);
            this.Controls.Add(this.dgvCompareData);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.btnOutToExcel);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompareData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOutToExcel;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.DataGridView dgvCompareData;
        private System.Windows.Forms.Label labCountTitle;
        private System.Windows.Forms.Label labCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn FNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn FBarcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn FFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FName;

    }
}

