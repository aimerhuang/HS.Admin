using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.SMS
{
    /// <summary>
    /// 短信类型
    /// </summary>
    /// <remarks>20013-6-26 杨浩 添加</remarks>
    public enum SmsPriority
    {
        /// <summary>
        /// 通知类短信，即时性要求高，包括但不限于：验证码、提货通知等
        /// </summary>
        Notification=5,

        /// <summary>
        /// 广告类短信，及时性要求不高，一般定时发送或者群发，包括但不限于：促销、广告。
        /// </summary>
        Advertisement=10
    }
}
