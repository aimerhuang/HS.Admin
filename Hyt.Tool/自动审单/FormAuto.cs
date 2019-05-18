using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 自动审单
{
    public partial class FormAuto : Form
    {
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();//暂停通知
        public FormAuto()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

           
            button2.Enabled = true;
            button1.Enabled = false;
            cancelTokenSource = new CancellationTokenSource();
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (cancelTokenSource.Token.IsCancellationRequested)//跳出循环
                    {
                        break;
                    }
                    var watch = new System.Diagnostics.Stopwatch();
                    watch.Start();
                    Hyt.BLL.Sys.SyJobMessageBo.Instance.AutoTask();
                    watch.Stop();
                    WriteLog("花费时间." + watch.ElapsedMilliseconds);
                    Thread.Sleep(1000);

                }
            }, cancelTokenSource.Token);
            
        }

        private delegate void WriteLogDelegate(string  r);//代理
        /// <summary>
        /// 实时记录日志
        /// </summary>
        /// <param name="r"></param>
        private void WriteLog(string content)
        {
            if (textBox1.InvokeRequired)//等待异步
            {
                WriteLogDelegate fc = new WriteLogDelegate(WriteLog);
                try
                {
                    this.Invoke(fc, content);//通过代理调用刷新方法
                }
                catch { }
            }
            else
            {
                int max = 5000;
                int page = 2000;
                if (textBox1.Lines.Length > max)
                {
                    textBox1.Text = string.Join(Environment.NewLine, textBox1.Lines.Skip(page).Take(max - page));
                }
                textBox1.AppendText(content + Environment.NewLine);
                textBox1.ScrollToCaret();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button1.Enabled = true;
            cancelTokenSource.Cancel();
            
        }
    }
}
