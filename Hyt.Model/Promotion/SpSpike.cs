using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Promotion
{
    public class SpSpike
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 促销编号
        /// </summary>
        public int PromotionSysNo { get; set; }
        /// <summary>
        /// 促销标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 开始实际
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 限购数
        /// </summary>
        public int RestrictionQuantity { get; set; }
        /// <summary>
        /// 已销售数
        /// </summary>
        public int SaleQuantity { get; set; }
        /// <summary>
        /// 关注公众号
        /// </summary>
        public int IsAttentionNoPublic { get; set; }
        /// <summary>
        /// 回答问题
        /// </summary>
        public int IsAnswerProblem { get; set; }
        /// <summary>
        /// 问题列表
        /// </summary>
        public string ProblemList { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status {get;set;}
        /// <summary>
        /// 取消过期天数
        /// </summary>
        public int CancelDay { get; set; }
        /// <summary>
        /// 启用秒杀资格设置
        /// </summary>
        public int IsQualificationSet { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateDate { get; set; }
    }
}
