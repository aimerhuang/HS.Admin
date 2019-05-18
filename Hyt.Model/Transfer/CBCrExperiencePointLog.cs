using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 客户经验积分日志扩展属性类
    /// </summary>
    /// <remarks>
    /// 2013-07-15 苟治国 创建
    /// </remarks>
    public class CBCrExperiencePointLog:CrExperiencePointLog
    {
        /// <summary>
        /// 系统用户名称
        /// </summary>
        /// <remarks>
        /// 2013-07-15 苟治国 创建
        /// </remarks>
        public string UserName { get; set; }

        /// <summary>
        /// 会员账号
        /// </summary>
        /// <remarks>
        /// 2013-07-17 苟治国 创建
        /// </remarks>
        public string CustomerAccount { get; set; }
    }
}
