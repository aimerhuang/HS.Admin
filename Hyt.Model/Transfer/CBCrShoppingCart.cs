using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Model
{
    /// <summary>
    /// 购物车组合对象
    /// </summary>
    /// <remarks>2013-8-12 吴文强 添加</remarks>
    public class CBCrShoppingCartItem : CrShoppingCartItem
    {
        /// <summary>
        /// 获取缩略图委托 
        /// </summary>
        /// <remarks>2013-10-30 杨浩 添加</remarks>
        public Func<string> GetThumbnail {private get; set; }

        /// <summary>
        /// 商品利润
        /// </summary>
        public decimal Catle { get; set; }
 
        /// <summary>
        /// 单个商品利润
        /// </summary>
        public decimal UnitCatle { get; set; }


        /// <summary>
        /// 商品原单价
        /// </summary>
        public decimal SalesUnitPrice { get; set; }

        /// <summary>
        /// 销售总金额(优惠前金额，原单价*数量)
        /// </summary>
        public decimal SaleTotalAmount { get; set; }

        /// <summary>
        /// 折扣金额
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// 商品缩略图
        /// </summary>
        public string Thumbnail 
        {
            get { return GetThumbnail != null ? GetThumbnail() : string.Empty; }
            set { }
        }

        /// <summary>
        /// 推荐展示规则
        /// </summary>
        public string Specification 
        {
            get { return ""; }
            set { }
        }

        /// <summary>
        /// 前台促销提示
        /// </summary>
        public IList<SpPromotionHint> PromotionHints { get; set; }
    }
}
