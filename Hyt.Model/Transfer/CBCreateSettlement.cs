using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Transfer;

namespace Hyt.Model
{
    /// <summary>
    /// 生成结算单json回传对象
    /// </summary>
    /// <remarks>2013-07-12 黄伟 创建</remarks>
    public class CBCreateSettlement:BaseEntity
    {
        
        /// <summary>
        /// 配送单系统编号集合
        /// </summary>
        public int[] DeliverySysNos { get; set; }

        /// <summary>
        /// 出库单信息集合
        /// </summary>
        public IList<StockOutInfo> StockOutInfos { get; set; }

        /// <summary>
        /// 操作人员系统编号
        /// </summary>
        public int OperatorSysNo { get; set; }

        /// <summary>
        /// 结算单备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 退货订单相关信息-重新计算价格
        /// </summary>
        public CBRMAOrderInfo RMAOrderInfo { get; set; }

    }

    /// <summary>
    /// 出库单信息-包含系统编号,配送单明细签收状态,签收商品集合,结算支付明细
    /// </summary>
    /// <remarks>2013-07-12 黄伟 创建</remarks>
    public class StockOutInfo : BaseEntity
    {
        /// <summary>
        /// 出库单系统编号
        /// </summary>
        public int StockOutSysNo { get; set; }

        ///// <summary>
        ///// 是否到付
        ///// </summary>
        //public int IsCOD { get; set; }

        /// <summary>
        /// 配送单明细签收状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 签收商品集合-for部分签收：如果是部分签收，则应传入
        /// </summary>
        public IList<SignedProductInfo> SignedProductInfos { get; set; }

        /// <summary>
        /// 结算支付明细
        /// </summary>
        public IList<PayItem> PayItemList { get; set; }
    }

    /// <summary>
    /// 签收商品
    /// </summary>
    /// <remarks>2013-07-12 黄伟 创建</remarks>
     public class SignedProductInfo:BaseEntity
    {

         /// <summary>
         /// 订单明细编号
         /// </summary>
         public int SysNo { get; set; }

         /// <summary>
         /// 签收数量
         /// </summary>
         public int Qty { get; set; }

     }

     /// <summary>
     /// 退货的订单信息-重新计算价格-后期结算修改后,此项暂未再使用
     /// </summary>
     /// <remarks>2013-09-24 黄伟 创建</remarks>
     public class CBRMAOrderInfo : BaseEntity
     {
         /// <summary>
         /// 订单编号
         /// </summary>
         public int OrderSysNo { get; set; }

         /// <summary>
         /// 签收的订单明细编号和退货商品数量
         /// </summary>
         public List<CBRMAOrderItemInfo> lstRMAOrderItemInfo { get; set; }

     }

     /// <summary>
     /// 退货的订单明细信息-页面重新计算价格
     /// </summary>
     /// <remarks>2013-09-24 黄伟 创建</remarks>
     public class CBRMAOrderItemInfo : BaseEntity
     {
         /// <summary>
         /// 订单明细编号
         /// </summary>
         public int OrderItemSysNo { get; set; }

         /// <summary>
         /// 退货数量
         /// </summary>
         public int Qty { get; set; }

     }
}
