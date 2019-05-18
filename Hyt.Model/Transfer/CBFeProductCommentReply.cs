using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 大宗采购扩展属性类
    /// </summary>
    /// <remarks>
    /// 2013-07-11 杨晗 创建
    /// </remarks>
    [Serializable]
    public class CBFeProductCommentReply : FeProductCommentReply
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 会员名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 会员手机号
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string HeadImage { get; set; }
    }
}
