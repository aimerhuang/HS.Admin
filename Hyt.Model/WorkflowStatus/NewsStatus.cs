using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 新闻状态
    /// </summary>
    /// <remarks>2016-06-06 罗远康 创建</remarks>
     public class NewsStatus
    {
        /// <summary>
        /// 新闻分类状态
        /// 数据表:PdCategory 字段:Status
        /// </summary>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public enum 新闻分类状态
        {
            有效 = 1,
            无效 = 0,
        }
    }
}
