
namespace Hyt.Model.Transfer
{
    public class CBFnOnlinePayment : FnOnlinePayment
    {
        /// <summary>
        /// 客户付款账户
        /// </summary>
        public string CusPaymentCode { get; set; }

        /// <summary>
        /// 支付方式名称(ui显示)
        /// </summary>
        public string PaymentName { get; set; }

        /// <summary>
        /// 交易金额合计(ui显示)
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 创建来源(ui显示)
        /// </summary>
        public string CreatedSource
        {
            get { return CreatedBy == 0 ? "系统添加" : "员工添加"; }
        }
    }
}
