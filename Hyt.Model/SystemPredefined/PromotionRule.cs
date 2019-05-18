using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.SystemPredefined
{
    /// <summary>
    /// 促销规则 预设值
    /// 数据表：SpPromotionRule
    /// </summary>
    /// <remarks>2013-09-26 吴文强 创建</remarks>
    public static class PromotionRule
    {
        /// <summary>
        /// 组合套餐系统编号
        /// </summary>
        public static int 组合套餐 { get { return 1; } }

        /// <summary>
        /// 团购系统编号
        /// </summary>
        public static int 团购 { get { return 2; } }
    }
}
