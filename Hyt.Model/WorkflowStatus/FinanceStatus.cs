using System.ComponentModel;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 财务状态
    /// </summary>
    /// <remarks>2013-09-10 吴文强 创建</remarks>
    public class FinanceStatus
    {
        /// <summary>
        /// 收款单状态
        /// 数据表:FnReceiptVoucher 字段:Status
        /// </summary>
        /// <remarks>2013-06-18 吴文强 创建</remarks>
        public enum 收款单状态
        {
            待确认 = 10,
            已确认 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 收款单收入类型
        /// 数据表:FnReceiptVoucher 字段:IncomeType
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum 收款单收入类型
        {
            预付 = 10,
            到付 = 20,
        }

        /// <summary>
        /// 收款来源类型
        /// 数据表:FnReceiptVoucher 字段:Source
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum 收款来源类型
        {
            销售单 = 10,
            分销商保证金=20,
            退换货单 = 50,
            采购退货单 = 60,
        }

        /// <summary>
        /// 收款单明细状态
        /// 数据表:FnReceiptVoucherItem 字段:Status
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum 收款单明细状态
        {
            有效 = 1,
            无效 = 0,
        }

        /// <summary>
        /// 收款方类型
        /// 数据表:FnReceiptVoucherItem 字段:ReceivablesSideType
        /// </summary>
        /// <remarks>2013-11-05 吴文强 创建</remarks>
        public enum 收款方类型
        {
            财务中心 = 10,
            仓库 = 20,
            分销中心 = 30,
        }

        /// <summary>
        /// 付款来源类型
        /// 数据表:FnPaymentVoucher 字段:Source
        /// </summary>
        /// <remarks>
        /// 2013-07-11 吴文强 创建
        /// 2016-04-18 刘伟豪 添加，代理商提现单
        /// </remarks>
        public enum 付款来源类型
        {
            销售单 = 10,
            分销商保证金 = 20,
            退换货单 = 50,
            会员提现单 = 60,
            分销商提现单 = 70,
            采购单 = 75,
            代理商提现单 = 80,
        }

        /// <summary>
        /// 付款单状态
        /// 数据表:FnPaymentVoucher 字段:Status
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum 付款单状态
        {
            待付款 = 10,
            部分付款 = 15,
            已付款 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 付款单付款方式
        /// 数据表:FnPaymentVoucherItem 字段:PaymentType
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum 付款单付款方式
        {
            网银 = 10,
            支付宝 = 20,
            转账 = 30,
            现金 = 40,
            支票 = 50,
            易宝 = 60,
            微信 = 70,
            余额 = 80,
            分销商预存 = 110
        }

        /// <summary>
        /// 付款单明细状态
        /// 数据表:FnPaymentVoucherItem 字段:Status
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum 付款单明细状态
        {
            待付款 = 10,
            已付款 = 20,
            作废 = -10,
        }

        /// <summary>
        /// 付款方类型
        /// 数据表:FnPaymentVoucherItem 字段:PaymentToType
        /// </summary>
        /// <remarks>2013-11-05 吴文强 创建</remarks>
        public enum 付款方类型
        {
            财务中心 = 10,
            仓库 = 20,
            分销中心 = 30,
        }

        /// <summary>
        /// 网上支付单据来源
        /// 数据表:FnOnlinePayment 字段:Source
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum 网上支付单据来源
        {
            销售单 = 10,
        }

        /// <summary>
        /// 网上支付状态
        /// 数据表:FnOnlinePayment 字段:Status
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum 网上支付状态
        {
            有效 = 1,
            无效 = 0,
        }

        /// <summary>
        /// 发票状态
        /// 数据表:FnInvoice 字段:Status
        /// </summary>
        /// <remarks>2013-07-11 吴文强 创建</remarks>
        public enum 发票状态
        {
            待开票 = 10,
            已开票 = 20,
            已取回 = 30,
            作废 = -10,
        }

        /// <summary>
        /// 是否默认收款科目
        /// 数据表:FnReceiptTitleAssociation 字段:IsDefault
        /// </summary>
        /// <remarks>2013-10-08 吴文强 创建</remarks>
        public enum 是否默认收款科目
        {
            是 = 1,
            否 = 0,
        }

        #region 自定义状态
        /// <summary>
        /// 订单发票状态
        /// </summary>
        /// <remarks>2013-10-29 吴文强 创建</remarks>
        public enum 订单发票状态
        {
            无发票 = 0,
            有发票 = 1,
            /// <summary>
            /// 已过期的发票在退货时需自动作为发票遗失处理（扣出发票扣款）
            /// </summary>
            已过期 = -1,
        }
        #endregion

        /// <summary>
        /// 第三方财务对账来源
        /// 数据表:FnThirdpartyReconciliation 字段:Source
        /// </summary>
        public enum 第三方财务对账来源
        {
            支付宝=10,
        }

        /// <summary>
        /// 第三方财务对账状态
        /// 数据表:FnThirdpartyReconciliation 字段:Status
        /// </summary>
        public enum 第三方财务对账状态
        {
            待对账 = 10,
            已对账 = 20,
            失败=-10
        }
        /// <summary>
        /// 提现支付状态 0为未支付 1为支付
        /// 数据表:CrPredepositCash 字段:PdcPayState
        /// </summary>
        public enum 提现支付状态
        {
            未支付 = 0,
            已支付 = 1,
        }
    }
}
