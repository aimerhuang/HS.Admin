using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Model
{
    /// <summary>
    /// 促销提示（用于商品详情展示页的提示）
    /// </summary>
    /// <remarks>2014-01-14 吴文强 创建</remarks>
    [Serializable]
    public class SpPromotionHint
    {
        /// <summary>
        /// 促销系统编号
        /// </summary>
        public int PromotionSysNo { get; set; }

        /// <summary>
        /// 促销规则类型
        /// </summary>
        public int RuleType { get; set; }

        /// <summary>
        /// 前台展示文本
        /// </summary>
        public string FrontText { get; set; }

        /// <summary>
        /// 活动主题Url
        /// </summary>
        public string SubjectUrl { get; set; }

        /// <summary>
        /// 来源系统编号(团购,组合主表系统编号)
        /// </summary>
        public int SourceSysNo { get; set; }
        /// <summary>
        /// 折扣金额
        /// </summary>
        public decimal DiscountAmount { get; set; }
    }
}
