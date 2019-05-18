using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyt.Service.Task.Core;

namespace Hyt.Service.Task.Tasks
{
    /// <summary>
    /// 定时执行Eas同步队列
    /// </summary>
    /// <remarks>2014-4-9 杨浩 创建</remarks>
    /// <remarks>2014-08-06 杨浩 修改并行执行</remarks>
    [Description("定时执行Eas同步队列")]
    public class EasSyncLogTask : ITask
    {
        static object lockthis = new object();
        static List<int> synclist = new List<int>();

        public void Execute(object state)
        {
            int count=10;
            int maxDegree = 1;
            var array = state.ToString().Split(',');

            if (array.Length == 2)
            {
                int.TryParse(array[0], out count);
                int.TryParse(array[1], out maxDegree);
            }
            
            var list = Hyt.BLL.Sys.EasBo.Instance.GetSyncList(count);
            if (list == null || list.Count == 0)
                return;

            var parallel = new List<List<Model.EasSyncLog>>();
            int index = 0;
            for (; index < list.Count; index++)
            {
                var item = list[index];
                var i = list.FindAll(x => x.FlowIdentify == item.FlowIdentify);
                parallel.Add(i);
                index = list.FindLastIndex(x => x.FlowIdentify == item.FlowIdentify);
            }
            Start(parallel, maxDegree);
        }

        /// <summary>
        /// 执行并行计划
        /// </summary>
        /// <param name="parallel">并行集合</param>
        /// <param name="maxDegree">最大并行数</param>
        /// <remarks>2014-08-06 杨浩 创建</remarks>
        private static void Start(IList<List<Model.EasSyncLog>> parallel, int maxDegree)
        {
            var client = Extra.Erp.Kis.KisProviderFactory.CreateProvider();
            var r = Parallel.For(0, parallel.Count, new ParallelOptions {MaxDegreeOfParallelism = maxDegree}, (i) =>
            {
                for (int index = 0; index < parallel[i].Count; index++)
                {
                    lock (lockthis)
                    {
                        if (synclist.Contains(parallel[i][index].SysNo))
                        {
                            return;
                        }
                        synclist.Add(parallel[i][index].SysNo);
                    }
                    var result = client.Resynchronization(parallel[i][index].SysNo);
                    synclist.Remove(parallel[i][index].SysNo);
                    //StatusCode == "9999"  标示已经导入过了
                    if (result.Status || result.StatusCode == "9999")
                    {
                        continue;
                    }
                    break;
                }
            });
        }
    }
}
