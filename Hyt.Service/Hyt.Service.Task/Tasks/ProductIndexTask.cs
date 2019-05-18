using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Service.Task.Core;

namespace Hyt.Service.Task.Tasks
{
    /// <summary>
    /// 生成商品索引
    /// </summary>
    public class ProductIndexTask : ITask
    {
        /// <summary>
        /// 生成商品索引
        /// </summary>
        /// <param name="state"></param>
        /// <remarks>2013-01-21 苟治国 添加</remarks>
        public void Execute(object state)
        {
            Hyt.BLL.Sys.SyTaskBo.Instance.ProductIndex();
        }
    }
}
