using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 等级日志扩展属性类
    /// </summary>
    /// <remarks>
    /// 2013-07-15 苟治国 创建
    /// </remarks>
    public class CBCrLevelLog:CrLevelLog
    {
        /// <summary>
        /// 原等级编号
        /// </summary>
        /// <remarks>
        /// 2013-07-15 苟治国 创建
        /// </remarks>
        public string oldLevelName { get; set; }

        /// <summary>
        /// 新等级编号
        /// </summary>
        /// <remarks>
        /// 2013-07-15 苟治国 创建
        /// </remarks>
        public string NewLevelName { get; set; }

        /// <summary>
        /// 系统用户名称
        /// </summary>
        /// <remarks>
        /// 2013-07-15 苟治国 创建
        /// </remarks>
        public string UserName { get; set; }
    }
}
