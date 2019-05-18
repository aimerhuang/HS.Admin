using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 等级积分日志扩展属性类
    /// </summary>
    /// <remarks>
    /// 2013-07-15 苟治国 创建
    /// </remarks>
    public class CBCrLevelPointLog:CrLevelPointLog
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

        /// <summary>
        /// 增加积分和
        /// </summary>
        /// <remarks>
        /// 2013-07-17 苟治国 创建
        /// </remarks>
        public string IncreasedSum { get; set; }

        /// <summary>
        /// 减少积分和
        /// </summary>
        /// <remarks>
        /// 2013-07-17 苟治国 创建
        /// </remarks>
        public string DecreasedSum { get; set; }
        
    }
}
