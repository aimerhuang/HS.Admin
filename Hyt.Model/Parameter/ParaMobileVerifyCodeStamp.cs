using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 手机验证码时间戳对象
    /// </summary>
    /// <remarks>2013-09-24 邵斌 创建</remarks>
    public class ParaMobileVerifyCodeStamp
    {
        /// <summary>
        /// 时间戳加密后字符串
        /// </summary>
        public string StampString { get; set; }

        /// <summary>
        /// 时间戳时间
        /// </summary>
        public DateTime StampDate { get; set; }
    }
}
