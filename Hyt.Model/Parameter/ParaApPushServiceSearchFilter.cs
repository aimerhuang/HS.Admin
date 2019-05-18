using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Transfer;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 推送消息参数对象
    /// </summary>
    /// <remarks>2014-01-16 邵斌 创建</remarks>
    public class ParaApPushServiceSearchFilter : CBApPushService
    {
        /// <summary>
        /// 分页表示符
        /// </summary>
        public int id { get; set; }
    }
}
