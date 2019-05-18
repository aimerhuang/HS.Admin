using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 意见反馈管理扩展类
    /// </summary>
    /// <remarks>2013-09-03 沈强 创建</remarks>
    public class CBCrFeedback : CrFeedback
    {
        /// <summary>
        /// 意见类型
        /// </summary>
        /// <remarks>2013-09-03 沈强 创建</remarks>
        public string SuggestType { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        /// <remarks>2013-09-03 沈强 创建</remarks>
        public string CreateName { get; set; }

        /// <summary>
        /// 客户帐号
        /// </summary>
        /// <remarks>2013-09-03 沈强 创建</remarks>
        public string Account { get; set; }
    }
}
