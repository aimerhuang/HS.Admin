using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Util.JPush
{
    /// <summary>
    /// 应用平台
    /// </summary>
    /// <remarks>2014-01-17 邵斌 创建</remarks>
    public enum PlatformEnum
    {
        /// <summary>
        /// 全部设备
        /// </summary>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        All = 0,

        /// <summary>
        /// Android设备
        /// </summary>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        Android = 1,

        /// <summary>
        /// 苹果IOS系统设备
        /// </summary>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        Ios = 2
    }
}
