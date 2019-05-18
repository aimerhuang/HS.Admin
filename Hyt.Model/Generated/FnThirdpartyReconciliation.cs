
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    ///财务对账
    /// </summary>
    /// <remarks>
    ///  2014-08-21 杨浩 T4生成
    /// </remarks>
    public partial class FnThirdpartyReconciliation : BaseEntity
    {
        ///<summary>
        ///系统编号
        ///</summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        ///<summary>
        ///来源:支付宝(10)
        ///</summary>
        [Description("来源:支付宝(10)")]
        public int Source { get; set; }

        ///<summary>
        ///财务流水号
        ///</summary>
        [Description("财务流水号")]
        public string FnNo { get; set; }

        ///<summary>
        ///业务流水号
        ///</summary>
        [Description("业务流水号")]
        public string OperationNo { get; set; }

        ///<summary>
        ///商户订单号
        ///</summary>
        [Description("商户订单号")]
        public string TraderNo { get; set; }

        ///<summary>
        ///商品名称
        ///</summary>
        [Description("商品名称")]
        public string ProductName { get; set; }

        ///<summary>
        ///交易时间
        ///</summary>
        [Description("交易时间")]
        public DateTime TradeDate { get; set; }

        ///<summary>
        ///对方账号
        ///</summary>
        [Description("对方账号")]
        public string BuyerAccount { get; set; }

        ///<summary>
        ///金额
        ///</summary>
        [Description("金额")]
        public decimal Amount { get; set; }

        ///<summary>
        ///对账时间
        ///</summary>
        [Description("对账时间")]
        public DateTime CheckDate { get; set; }

        ///<summary>
        ///备注
        ///</summary>
        [Description("备注")]
        public string Remarks { get; set; }

        ///<summary>
        ///状态:待对账(10),已对账(20),失败(-10)
        ///</summary>
        [Description("状态:待对账(10),已对账(20),失败(-10)")]
        public int Status { get; set; }

        ///<summary>
        ///创建人
        ///</summary>
        [Description("创建人")]
        public int CreatedBy { get; set; }

        ///<summary>
        ///创建时间
        ///</summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }

        ///<summary>
        ///最后更新人
        ///</summary>
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }

        ///<summary>
        ///最后更新时间
        ///</summary>
        [Description("最后更新时间")]
        public DateTime LastUpdateDate { get; set; }


    }
}
