using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 会员咨询查询筛选字段
    /// </summary>
    /// <remarks>
    /// 2013-08-21 郑荣华 创建
    /// </remarks>
    public class ParaCrCustomerQuestionFilter
    {
        /// <summary>
        /// 客户系统编号
        /// </summary>    
        public int? CustomerSysNo { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public int? ProductSysNo { get; set; }

        /// <summary>
        /// 咨询类型：商品（10）、支付（20）、配送（30）、其它
        /// </summary>

        public int? QuestionType { get; set; }

        /// <summary>
        /// 状态：待回复（10）、已回复（20）、作废（－10）
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 咨询时间（起）
        /// </summary>    
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 咨询时间（止）
        /// </summary>
      
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 产品名称 模糊查询
        /// </summary>   
        public string ProductName { get; set; }
    }
}
