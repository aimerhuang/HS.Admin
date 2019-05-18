using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Hyt.Service.Task.Core;
using System.Threading;

namespace Hyt.Service.Task.Tasks
{
    /// <summary>
    /// 任务测试
    /// </summary>
    [Description("任务测试,记录运行情况")]
    public class TestTask:ITask
    {
        /// <summary>
        /// 任务测试
        /// </summary>
        /// <param name="state"></param>
        /// <remarks>2013-01-21 苟治国 追加</remarks>
        public void Execute(object state)
        {
            Thread.Sleep(8000);
            System.Diagnostics.Debug.WriteLine("任务测试，时间："+DateTime.Now+" 当前线程:"+Thread.CurrentThread.Name);
            //throw new Exception("测试错误处理");
        }
    }
}
