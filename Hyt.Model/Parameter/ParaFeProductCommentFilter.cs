using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{

    /// <summary>
    /// 商品评论筛选条件
    /// </summary>
    /// <remarks>2013-08-12 邵斌 创建</remarks>
    public class ParaFeProductCommentFilter
    {
        /// <summary>
        /// 商品系统编号
        /// </summary>
        public int? ProductSysNo { get; set; }

        /// <summary>
        /// 是否晒单 是（1）、否（0）
        /// </summary>
        public int? IsShare { get; set; }

        /// <summary>
        /// 开始分数
        /// </summary>
        public int? StartSocre { get; set; }

        /// <summary>
        /// 结束分数
        /// </summary>
        public int? EndSocre { get; set; }

        /// <summary>
        /// 是否评论 是（1）、否（0）
        /// </summary>
        public int? IsComment { get; set; }

        /// <summary>
        /// 状态 待审（10）、已审（20）、作废（－10）
        /// </summary>
        public int? CommentStatus { get; set; }

        /// <summary>
        /// 晒单状态：待审（10）、已审（20）、作废（－10）
        /// </summary>      
        public int? ShareStatus { get; set; }

        /// <summary>
        /// 是否精华 是（1）、否（0） 
        /// </summary>
        public int? IsBest { get; set; }

        /// <summary>
        /// 是否置顶 是（1）、否（0）
        /// </summary>
        public int? IsTop { get; set; }

        /// <summary>
        /// 评论开始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 评论结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 会员名称
        /// </summary>
        public string CustomerName { get; set; }

  
    }
}
