using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 订单商品明细详情
    /// </summary>
    /// <remarks>
    /// 2013-07-26 朱成果 创建
    /// </remarks>
    public class CBSoOrderItem : SoOrderItem
    {
        ///<summary>
        ///商品编号
        ///</summary>
        public string ErpCode { get; set; }

        ///<summary>
        ///商品名称
        ///</summary>
        public string EasName { get; set; }

        ///<summary>
        ///商品条形码
        ///</summary>
        public string BarCode { get; set; }

        ///<summary>
        ///库存数量
        ///</summary>
        public decimal StockQuantity { get; set; }
        /// <summary>
        /// 返利比例
        /// </summary>
        public string RebateRtio { get; set; }
        /// <summary>
        /// 订单海关推送状态
        /// </summary>
        public decimal OperatFee { get; set; }

        /// <summary>
        /// 商品重量
        /// </summary>
        public decimal GrosWeight { get; set; }
        public decimal NetWeight { get; set; }

        public string OrginCountry { get; set; }
    }

    public class CDSoOrderItem : SoOrderItem
    {
        public string OrderNo { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
