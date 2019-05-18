using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Hyt.Model;
using System.Data.SqlClient;

namespace Hyt.Tool.ImageBuilder
{
    public partial class Form1 : Form
    {
        ProductImageConfig productImageConfig = Hyt.Tool.ImageBuilder.Config.Config.Instance.GetProductImageConfig();//图片配置信息
        string sourceFolder;//要处理的图片所在文件夹
        string targetFolder;//处理过后的图片保存文件夹
        int intOther1;//其他尺寸图片大小

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSource_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择要转换的文件夹";
            dialog.ShowNewFolderButton = false;
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var folder = dialog.SelectedPath;
                if (!string.IsNullOrEmpty(folder))
                {
                    sourceFolder = folder;
                    tbSourceFolder.Text = folder;
                }
            }
        }

        private void btnTarget_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择要保存的文件夹";
            dialog.ShowNewFolderButton = true;
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var folder = dialog.SelectedPath;
                if (!string.IsNullOrEmpty(folder))
                {
                    targetFolder = folder;
                    tbTargetFolder.Text = folder;
                }
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sourceFolder) || string.IsNullOrEmpty(targetFolder))
            {
                MessageBox.Show("请选择输入、输出文件夹", "操作提示", MessageBoxButtons.OK);
                return;
            }

            if (cbOther.Checked)
            {
                if (string.IsNullOrEmpty(mtbO1.Text))
                {
                    MessageBox.Show("当选择其他尺寸缩略图时，请输入相应的尺寸！", "操作提示", MessageBoxButtons.OK);
                    return;
                }
                intOther1 = int.Parse(mtbO1.Text);
            }

            var start = new ParameterizedThreadStart(DoWork);
            var thread = new Thread(start);
            thread.Start(new string[] { sourceFolder, targetFolder });
            btnThumbnail.Enabled = false;
            btnConvert.Enabled = false;
        }

        #region 缩略图
        private List<string> ls800;
        private List<string> ls460;
        private DirectoryInfo sourceDir;
        private List<string> ls;
        private void btnThumbnail_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sourceFolder) || string.IsNullOrEmpty(targetFolder))
            {
                MessageBox.Show("请选择输入、输出文件夹", "操作提示", MessageBoxButtons.OK);
                return;
            }
            var start = new ParameterizedThreadStart(DoWorkProportion);
            var thread = new Thread(start);
            thread.Start(new string[] { sourceFolder, targetFolder });
            btnThumbnail.Enabled = false;
            btnConvert.Enabled = false;
        }
        #endregion

        delegate void UpdateProgressDelegate(int count, int progress);

        private void UpdateProgress(int count, int progress)
        {
            if (status.InvokeRequired)
            {
                Invoke(new UpdateProgressDelegate(UpdateProgress), new object[] { count, progress });
            }
            else
            {
                pbProgress.Maximum = count;
                pbProgress.Value = progress;
                pbProgress.Visible = true;

                lbProgress.Text = (progress * 100 / count).ToString() + "%";

                if (count == progress){
                    btnConvert.Enabled = true;
                btnThumbnail.Enabled = true;
                }
            }
        }

        #region 批量生成比例图
        private void DoWork(object arg)
        {
            string[] arr = arg as string[];

            var sourceFolder = arr[0];
            var targetFolder = arr[1];

            string[] files = Directory.GetFiles(sourceFolder, "*.jpg");

            //创建新连接
            using (OracleConnection conn = new OracleConnection(ConfigurationManager.AppSettings["OracleConnectionString"].ToString()))
            {
                try
                {
                    conn.Open();
                    OracleCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT SYSNO FROM PdProduct ORDER BY SYSNO";

                    DataTable dt = new DataTable();
                    OracleDataAdapter adapter = new OracleDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dt);
                    if (dt != null)
                    {
                        //调整进度
                        UpdateProgress(dt.Rows.Count, 0);

                        int i = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            i++;
                            string sysNo = dr["sysNo"].ToString();
                            string filePath = string.Format(sourceFolder + "\\" + dr["sysNo"] + ".jpg");

                            //检查文件是否正在
                            if (System.IO.File.Exists(filePath))
                            {
                                if (cb460.Checked) ImageUtil.CreateThumbnail(sourceFolder + "\\" + sysNo + ".jpg", targetFolder + "\\Image460\\" + sysNo + ".jpg", 460, 460, ImageUtil.ThumbnailMode.WidthHeighLimitted);
                                if (cb240.Checked) ImageUtil.CreateThumbnail(sourceFolder + "\\" + sysNo + ".jpg", targetFolder + "\\Image240\\" + sysNo + ".jpg", 240, 240, ImageUtil.ThumbnailMode.WidthHeighLimitted);
                                if (cb200.Checked) ImageUtil.CreateThumbnail(sourceFolder + "\\" + sysNo + ".jpg", targetFolder + "\\Image200\\" + sysNo + ".jpg", 200, 200, ImageUtil.ThumbnailMode.WidthHeighLimitted);
                                if (cb180.Checked) ImageUtil.CreateThumbnail(sourceFolder + "\\" + sysNo + ".jpg", targetFolder + "\\Image180\\" + sysNo + ".jpg", 180, 180, ImageUtil.ThumbnailMode.WidthHeighLimitted);
                                if (cb120.Checked) ImageUtil.CreateThumbnail(sourceFolder + "\\" + sysNo + ".jpg", targetFolder + "\\Image120\\" + sysNo + ".jpg", 120, 120, ImageUtil.ThumbnailMode.WidthHeighLimitted);
                                if (cb100.Checked) ImageUtil.CreateThumbnail(sourceFolder + "\\" + sysNo + ".jpg", targetFolder + "\\Image100\\" + sysNo + ".jpg", 100, 100, ImageUtil.ThumbnailMode.WidthHeighLimitted);
                                if (cb80.Checked) ImageUtil.CreateThumbnail(sourceFolder + "\\" + sysNo + ".jpg", targetFolder + "\\Image80\\" + sysNo + ".jpg", 80, 80, ImageUtil.ThumbnailMode.WidthHeighLimitted);
                                if (cb60.Checked) ImageUtil.CreateThumbnail(sourceFolder + "\\" + sysNo + ".jpg", targetFolder + "\\Image60\\" + sysNo + ".jpg", 60, 60, ImageUtil.ThumbnailMode.WidthHeighLimitted);
                            }

                            //调整进度
                            UpdateProgress(dt.Rows.Count, i);
                        }
                        
                    }
                    conn.Close();
                    //调整进度
                    UpdateProgress(dt.Rows.Count, dt.Rows.Count);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        #endregion

        #region 批量生成缩略图
        private void DoWorkProportion(object arg)
        {
            try
            {
                string[] arr = arg as string[];

                var sourceFolder = arr[0];

                #region 装载图片
                ls800 = new List<string>();
                sourceDir = new DirectoryInfo(sourceFolder);
                foreach (FileSystemInfo fs in sourceDir.GetFileSystemInfos())
                {
                    int staridx = fs.FullName.LastIndexOf('\\');
                    string imagefile = fs.FullName.Substring(staridx + 1);
                    int idx = imagefile.LastIndexOf(".");
                    ls800.Add(imagefile.Substring(0, idx));
                }
                #endregion

                //创建新连接
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["OracleConnectionString"].ToString()))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "SELECT * FROM PdProduct where DealerSysNo=108  ORDER BY SYSNO";

                        DataTable dt = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dt);

                        if (dt != null)
                        {
                            //调整进度
                            UpdateProgress(dt.Rows.Count, 0);

                            int i = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                string sysNo = dr["sysNo"].ToString();
                                //系统编号

                                #region 查找同一个产品的其它图片
                                ls = new List<string>();
                                foreach (var drr in ls800)
                                {
                                    string[] arrr = drr.Split('_');
                                    if (drr == sysNo)
                                    {
                                        ls.Add(drr);
                                    }
                                    if (arrr.Length > 1)
                                    {
                                        if (arrr[1] == sysNo)
                                        {
                                            ls.Add(drr);
                                        }
                                    }
                                }
                                #endregion

                                int k = 0;
                                foreach (var itemNo in ls)
                                {
                                    
                                    string fileName = NewFileName(".jpg");
                                    string fileSmallName = fileName + ".small.jpg";
                                    string filePath = string.Format(@"{0}\\{1}.jpg", sourceFolder, itemNo);

                                    //数据库格式文件
                                    string dbbasePath = string.Format(productImageConfig.ProductImagePathFormat, "{1}", fileName);
                                    //检查文件是否正在
                                    if (System.IO.File.Exists(filePath))
                                    {
                                        ImageUtil.CreateThumbnail(sourceFolder + "\\" + itemNo + ".jpg", targetFolder + "\\Base\\" + fileName, 800, 800, ImageUtil.ThumbnailMode.WidthHeighLimitted);
                                        ImageUtil.CreateThumbnail(sourceFolder + "\\" + itemNo + ".jpg", targetFolder + "\\Big\\" + fileName, 460, 460, ImageUtil.ThumbnailMode.WidthHeighLimitted);
                                        ImageUtil.CreateThumbnail(sourceFolder + "\\" + itemNo + ".jpg", targetFolder + "\\Small\\" + fileSmallName, 100, 100, ImageUtil.ThumbnailMode.WidthHeighLimitted);

                                        int status = 0;
                                        if (k == 0)
                                        {
                                            status = 1;
                                        }
                                        string sql =
                                            string.Format(
                                                "insert into PdProductImage(ProductSysNo,ImageUrl,Status,DisplayOrder) values({0}, '{1}', {2}, {3})",
                                                sysNo, "{0}" + dbbasePath, status, k);

                                        cmd.CommandText = sql;
                                        cmd.ExecuteNonQuery();
                                        k++;
                                    }
                                }
                                i++;
                                UpdateProgress(dt.Rows.Count, i);
                            }
                            UpdateProgress(dt.Rows.Count, dt.Rows.Count);
                        }
                        conn.Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
            
        }        
        #endregion

        #region 生成新文件名称
        private void mtbO1_KeyUp(object sender, KeyEventArgs e)
        {
            lblO1.Text = mtbO1.Text;
        }

        private string NewFileName(string fileExtension)
        {
            return Guid.NewGuid().ToString("N") + fileExtension;
        }
        private string NewFileNames()
        {
            return Guid.NewGuid().ToString("N");
        }
        private byte[] SetImageToByteArray(string fileName)
        {
            byte[] image = null;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open);
                FileInfo fileInfo = new FileInfo(fileName);
                int streamLength = (int)fs.Length;
                image = new byte[streamLength];
                fs.Read(image, 0, streamLength);
                fs.Close();
                return image;
            }
            catch
            {
                return image;
            }
        }
        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
        private MemoryStream ByteToStream(byte[] mybyte)
        {
            MemoryStream mymemorystream = new MemoryStream(mybyte, 0, mybyte.Length);
            return mymemorystream;
        }
        #endregion

        /// <summary>
        /// 读取所有产品生成缩略图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
               

                //#region 装载图片
                //ls800 = new List<string>();
                //sourceDir = new DirectoryInfo(sourceFolder);
                //foreach (FileSystemInfo fs in sourceDir.GetFileSystemInfos())
                //{
                //    int staridx = fs.FullName.LastIndexOf('\\');
                //    string imagefile = fs.FullName.Substring(staridx + 1);
                //    int idx = imagefile.LastIndexOf(".");
                //    ls800.Add(imagefile.Substring(0, idx));
                //}
                //#endregion

                //创建新连接
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["OracleConnectionString"].ToString()))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();

                        cmd.CommandText = "SELECT  pimg.[ProductSysNo],pimg.[ImageUrl] FROM [PdProductImage] as pimg inner join PdProduct as p on p.sysNo=pimg.ProductSysNo  where p.DealerSysNo=108 and pimg.status=1";
                        //cmd.CommandText = "SELECT  pimg.[ProductSysNo],pimg.[ImageUrl] FROM [PdProductImage] as pimg inner join PdProduct as p on p.sysNo=pimg.ProductSysNo  where pimg.status=1";

                        DataTable dt = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dt);

                        if (dt != null)
                        {
                            //调整进度
                            UpdateProgress(dt.Rows.Count, 0);

                            int i = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                string sysNo = dr["ProductSysNo"].ToString();
                                //系统编号

                                string filePath = dr["ImageUrl"].ToString();

                            

                                int k = 0;
                             

                                     sourceFolder="D:\\RNBXiaoQuShangChao\\StaticFiles\\SG\\Images\\";
                                     
                                    string fileName =sysNo+".jpg";
                                    string fileSmallName = fileName + ".small.jpg";
                                    //string filePath = string.Format(@"{0}\\{1}.jpg", sourceFolder, itemNo);
                                    fileName= sourceFolder+string.Format(filePath,"", "Base").Replace("/", "\\");

                                    string saveFile = sourceFolder + "Product\\Image100\\" + sysNo + ".jpg";
                          
                                    //检查文件是否正在
                                    if (System.IO.File.Exists(fileName))
                                    {
                                       // ImageUtil.CreateThumbnail(sourceFolder + "\\" + itemNo + ".jpg", targetFolder + "\\Base\\" + fileName, 800, 800, ImageUtil.ThumbnailMode.WidthHeighLimitted);
                                       // ImageUtil.CreateThumbnail(sourceFolder + "\\" + itemNo + ".jpg", targetFolder + "\\Big\\" + fileName, 460, 460, ImageUtil.ThumbnailMode.WidthHeighLimitted);
                                       // ImageUtil.CreateThumbnail(sourceFolder + "\\" + itemNo + ".jpg", targetFolder + "\\Small\\" + fileSmallName, 100, 100, ImageUtil.ThumbnailMode.WidthHeighLimitted);

                                        ImageUtil.CreateThumbnail(fileName, saveFile, 240, 240, ImageUtil.ThumbnailMode.WidthHeighLimitted);


                                     
                                    
                                        k++;
                                    }
                                
                                i++;
                                UpdateProgress(dt.Rows.Count, i);
                            }
                            UpdateProgress(dt.Rows.Count, dt.Rows.Count);
                        }
                        conn.Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

    }
}
