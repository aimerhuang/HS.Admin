
namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 付款单
    /// </summary>
    /// <remarks>2013-07-19 朱家宏 创建</remarks>
    public class CBPaymentVoucher : FnPaymentVoucher
    {
        /// <summary>
        /// 待付金额
        /// </summary>
        public decimal DebtAmount
        {
            get { return PayableAmount - PaidAmount; }
        }
    }
}
