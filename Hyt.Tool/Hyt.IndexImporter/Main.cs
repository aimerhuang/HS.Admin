using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using Hyt.DataAccess.Provider;
using Hyt.Model;

namespace Hyt.IndexImporter
{
    public partial class Main : Form
    {
        private string INDEX_STORE_PATH = ConfigurationManager.AppSettings["LucenePath"];
        private Stopwatch watch = new Stopwatch();

        public Main()
        {
            InitializeComponent();

            #region  SqlServer.DataProvider 初始化

            lock (this)
            {
                ////Type type = Type.GetType("Hyt.DataAccess.SqlServer.DataProvider,Hyt.DataAccess.SqlServer");
                ////ProviderManager.Set<IDataProvider>(
                ////(IDataProvider)Activator.CreateInstance(type)
                //// );
                //Hyt.Infrastructure.Initialize.Init();
                ProviderManager.Set<IDataProvider>(
                    (IDataProvider)
                    Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle"))
                    );
            }

            #endregion

            toolStripStatusLabel1.Text ="索引存放路径:" + INDEX_STORE_PATH;
        }

        /// <summary>
        /// 生成索引事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>2014-1-8 杨浩 修改窗体假死(启用子线程,UI异步)</remarks>
        private void bnCreateIndex_Click(object sender, EventArgs e)
        {
            this.bnCreateIndex.Enabled = false;
            this.bnCreateIndex.Text = "正在生成索引";
            this.toolStripStatusLabel1.Text = "正在读取数据...";
            IList<PdProductIndex> lists = null;

            new Thread(() =>
                {
                    watch.Start();
                    lists = Hyt.BLL.Product.PdProductBo.Instance.GetAllProduct();
                    this.BeginInvoke(new Action(() =>
                        {
                            watch.Stop();
                            this.toolStripStatusLabel1.Text = "读取完毕耗时:" + watch.ElapsedMilliseconds + "毫秒";
                            if (lists == null)
                            {
                                MessageBox.Show("没有数据更新");
                                return;
                            }
                            watch.Reset();
                            int count = 0;
                            int MaxCount = lists.Count;

                            Hyt.Infrastructure.Lucene.ProductIndex.Instance.Path = INDEX_STORE_PATH;
                            Hyt.Infrastructure.Lucene.ProductIndex.Instance.CreateIndex(true);
                            Hyt.Infrastructure.Lucene.ProductIndex.Instance.MaxMergeFactor = 301;
                            Hyt.Infrastructure.Lucene.ProductIndex.Instance.MaxBufferedDocs = 301;

                            progressBar1.Value = 0;
                            Application.DoEvents();

                            foreach (var mode in lists)
                            {
                                watch.Start();

                                Hyt.Infrastructure.Lucene.ProductIndex.Instance.IndexString(mode);

                                watch.Stop();

                                count++;
                                progressBar1.Value = count*100/MaxCount;
                                lbProgress.Text = progressBar1.Value + "%";
                                Application.DoEvents();

                                if (count >= MaxCount)
                                {
                                    break;
                                }

                                if (count%300 == 0)
                                {
                                    Hyt.Infrastructure.Lucene.ProductIndex.Instance.CloseWithoutOptimize();
                                    Hyt.Infrastructure.Lucene.ProductIndex.Instance.CreateIndex(false);
                                    Hyt.Infrastructure.Lucene.ProductIndex.Instance.MaxMergeFactor = 301;
                                    Hyt.Infrastructure.Lucene.ProductIndex.Instance.MaxBufferedDocs = 301;
                                }
                            }

                            watch.Start();
                            Hyt.Infrastructure.Lucene.ProductIndex.Instance.Close();
                            watch.Stop();
                            this.bnCreateIndex.Enabled = true;
                            this.bnCreateIndex.Text = "重新生成索引";
                            this.toolStripStatusLabel1.Text = String.Format("插入{0}行数据,用时{1}秒", MaxCount,
                                                                       watch.ElapsedMilliseconds/1000 + "." +
                                                                       watch.ElapsedMilliseconds%1000);
                        }));

                }).Start();
        }
    }
}
