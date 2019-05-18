using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 产品满意度调查
    /// </summary>
    /// <remarks>
    /// 2013-11-19 邵斌 创建
    /// </remarks>
    [Serializable]
    public class CBReviewSatisfactionInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 商品评论id
        /// </summary>
        public int ReviewSysNo { get; set; }
        /// <summary>
        /// 外观设计
        /// </summary>
        public int DesignScore { get; set; }
        /// <summary>
        /// 参数性能
        /// </summary>
        public int PerformanceScore { get; set; }
        /// <summary>
        /// 材质用料
        /// </summary>
        public int Materialscore { get; set; }
        /// <summary>
        /// 操作便携
        /// </summary>
        public int OperateScore { get; set; }
        /// <summary>
        /// 性价比
        /// </summary>
        public int PriceScore { get; set; }
        /// <summary>
        /// 综合评价
        /// </summary>
        public int AppraiseScore { get; set; }
        /// <summary>
        /// 产品设计
        /// </summary>
        public string Design { get; set; }
    }
}
