using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 会员投诉回复扩展属性类
    /// </summary>
    /// <remarks>
    /// 2013-07-09 苟治国 创建
    /// </remarks>
    public class CBCrComplaintReply:CrComplaintReply
    {
        /// <summary>
        /// 客服、会员名称
        /// </summary>
        /// <remarks>
        /// 2013-07-09 苟治国 创建
        /// </remarks>
        public string UserName { get; set; }
    }
}
