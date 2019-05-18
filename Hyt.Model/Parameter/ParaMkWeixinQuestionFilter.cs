using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 微信咨询客服回复查询参数类
    /// </summary>
    /// <remarks>
    /// 2013-11-07 郑荣华 创建
    /// </remarks>
    public class ParaMkWeixinQuestionFilter
    {
        /// <summary>
        /// 微信号
        /// </summary>
        public string WeixinId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime;

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime;
    }
}
