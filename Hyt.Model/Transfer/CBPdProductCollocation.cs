using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 列表中商品
    /// </summary>
    /// <remarks>2013-07-09 邵斌 创建</remarks>
    public class CBProductListItem : PdProduct
    {
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 单个会员等级价格
        /// </summary>
        public decimal SingleCustomerLevelPrice { get; set; }

        /// <summary>
        /// 商品评分
        /// </summary>
        public decimal ProductCommentScore { get; set; }

        /// <summary>
        /// 商品评论次数
        /// </summary>
        public int CommentTimesCount { get; set; }

    }
}
