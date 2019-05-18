using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 退换货实体扩展
    /// </summary>
    /// <remarks>2013-07-11 朱成果 创建</remarks>
    public class CBRcReturn : RcReturn
    {
        /// <summary>
        /// 退换货商品明细
        /// </summary>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public List<RcReturnItem> RMAItems { get; set; }

        /// <summary>
        /// 门店（仓库）名称
        /// </summary>
        /// <remarks>2013-07-11 朱家宏 添加</remarks>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 后台门店（仓库）名称
        /// </summary>
        /// <remarks>2013-07-11 朱家宏 添加</remarks>
        public string BackWarehouseName { get; set; }

        /// <summary>
        /// 会员帐号
        /// </summary>
        /// <remarks>2013-07-11 朱家宏 添加</remarks>
        public string CustomerAccount { get; set; }

    }

    /// <summary>
    /// 退换货相关信息
    /// </summary>
    /// <remarks>2013-07-17 朱家宏 创建</remarks>
    public class CBRmaRelations
    {
        /// <summary>
        /// 入库单
        /// </summary>
        public WhStockIn StockIn { get; set; }

        /// <summary>
        /// 付款单
        /// </summary>
        public FnPaymentVoucher PaymentVoucher { get; set; }

        /// <summary>
        /// 退换货日志
        /// </summary>
        public IList<Transfer.CBRcReturnLog> RmaLogs { get; set; }
    }
    /// <summary>
    /// 退货货明细
    /// </summary>
    public class CBRmaReturnItem
    {
        /// <summary>
        /// 出库单明细编号
        /// </summary>
       public  int stockoutitemsysno { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
       public int ProductQuantity { get; set; }
      /// <summary>
      /// 退换货数量
      /// </summary>
       public int RmaQuantity { get; set; }
        /// <summary>
        /// 退换货状态
        /// </summary>
       public int RMAStatus { get; set; }

       /// <summary>
       /// 退换货编号
       /// </summary>
       public int RMAID { get; set; }
        /// <summary>
        /// 退换货类型
        /// </summary>
       public int RmaType { get; set; }
    }
}
