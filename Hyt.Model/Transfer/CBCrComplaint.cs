using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 会员投诉扩展属性类
    /// </summary>
    /// <remarks>
    /// 2013-07-09 苟治国 创建
    /// </remarks>
    public class CBCrComplaint:CrComplaint
    {
        /// <summary>
        /// 会员投诉会员名字
        /// </summary>
        /// <remarks>
        /// 2013-07-09 苟治国 创建
        /// </remarks>
        public string Name { get; set; }
        /// <summary>
        /// 会员投诉手机号
        /// </summary>
        /// <remarks>
        /// 2013-07-09 苟治国 创建
        /// </remarks>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 会员投诉回复类型
        /// </summary>
        /// <remarks>
        /// 2013-07-09 苟治国 创建
        /// </remarks>
        public int ReplyerType { get; set; }

    }
}
