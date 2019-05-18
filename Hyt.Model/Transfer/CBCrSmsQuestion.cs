using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 用户短信咨询传送类
    /// </summary>
    /// <remarks>2014-02-24 邵斌 创建</remarks>
    public class CBCrSmsQuestion : CrSmsQuestion
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        /// <remarks>2014-02-24 邵斌 创建</remarks>
        public string Customer { get; set; }

        /// <summary>
        /// 回复人账号
        /// </summary>
        /// <remarks>2014-02-24 邵斌 创建</remarks>
        public string AnswerName { get; set; }
    }
}
