using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// wcf传入参数:选择部分签收,获取订单总金额
    /// </summary>
    /// <remarks>2013-07-22 黄伟 创建</remarks>
    public class CBWCFParamGetAmount:BaseEntity
    {
        /// <summary>
        /// 出库单系统编号
        /// </summary>
        public int StockOutSysNo { get; set; }

        /// <summary>
        /// 会员系统编号
        /// </summary>
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 商品列表
        /// </summary>
        public IList<CBWCFParamGetAmountProduct> Items { get; set; }

    }

    /// <summary>
    /// 商品
    /// </summary>
    /// <remarks>2013-07-22 黄伟 创建</remarks>
    public class CBWCFParamGetAmountProduct : BaseEntity
    {
        /// <summary>
        /// 商品系统编号
        /// </summary>
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 签收数量
        /// </summary>
        public int SignQty { get; set; }

    }

    /// <summary>
    /// 该订单金额
    /// </summary>
    /// <remarks>2013-07-22 黄伟 创建</remarks>
    public class CBWCFParamGetAmountOrderAmount : BaseEntity
    {
        ///// <summary>
        ///// 商品总价
        ///// </summary>
        //public decimal TotalAmount { get; set; }

        ///// <summary>
        ///// 支付金额
        ///// </summary>
        //public decimal TotalAmountToPay { get; set; }

        /// <summary>
        /// 退回金额
        /// </summary>
        public decimal ReturnAmount { get; set; }

    }

}
