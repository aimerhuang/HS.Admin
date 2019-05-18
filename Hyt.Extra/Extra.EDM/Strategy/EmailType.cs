using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.EDM
{
    /// <summary>
    /// 邮件类型
    /// </summary>
    /// <remarks>
    /// 2014-01-13 何方 创建
    /// </remarks>
    public enum EmailType
    {
        /// <summary>
        /// 通知类邮件，即时性要求高
        /// </summary>
        Notification = 5,

        /// <summary>
        /// 广告类邮件，及时性要求不高
        /// </summary>
        Advertisement = 10
    }
}
