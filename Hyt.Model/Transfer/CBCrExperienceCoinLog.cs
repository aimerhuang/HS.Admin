using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 惠源币日志子类
    /// </summary>
    /// <remarks>
    /// 2013-07-15 杨晗 创建
    /// </remarks>
    public class CBCrExperienceCoinLog : CrExperienceCoinLog
    {
        /// <summary>
        /// 客户帐号
        /// </summary>
        /// <remarks>
        /// 2013-01-09 苟治国 追加 
        /// </remarks>
        public string CustomerAccount { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        /// <remarks>
        /// 2013-01-09 苟治国 追加 
        /// </remarks>
        public string CustomerName { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        /// <remarks>
        /// 2013-01-09 苟治国 追加 
        /// </remarks>
        public string CreatedByName { get; set; }
    }
}
