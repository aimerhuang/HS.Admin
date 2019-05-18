using System;
using Hyt.BLL.CRM;
using Hyt.BLL.Finance;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Order
{
    /// <summary>
    /// 订单扩展方法
    /// </summary>
    /// <remarks>2013-10-29 吴文强 创建</remarks>
    public static class SoOrderExtensions
    {
        /// <summary>
        /// 获取订单发票状态
        /// </summary>
        /// <param name="order">订单系统编号</param>
        /// <returns>订单发票状态</returns>
        /// <remarks>2013-10-29 吴文强 创建</remarks>
        public static FinanceStatus.订单发票状态 InvoiceStatus(this SoOrder order)
        {
            var invoiceStatus = FinanceStatus.订单发票状态.无发票;
            var invoice = FnInvoiceBo.Instance.GetOrderValidInvoice(order.SysNo);
            if (invoice != null)
            {
                var invoiceType = FnInvoiceBo.Instance.GetFnInvoiceType(invoice.InvoiceTypeSysNo);

                //最后更新时间为开票时间
                if (DateTime.Now > invoice.LastUpdateDate.AddDays(invoiceType.OverTime))
                {
                    invoiceStatus = FinanceStatus.订单发票状态.已过期;
                }
                else
                {
                    invoiceStatus = FinanceStatus.订单发票状态.有发票;
                }
            }
            return invoiceStatus;
        }

        /// <summary>
        /// 获取订单已使用的促销系统编号集合
        /// </summary>
        /// <param name="order">订单</param>
        /// <returns>订单已使用的促销系统编号集合</returns>
        /// <remarks>2013-11-14 吴文强 创建</remarks>
        public static int[] GetUsedPromotionSysNo(this SoOrder order)
        {
            return CrShoppingCartConvertBo.Instance.GetUsedPromotionSysNo(order.SysNo);
        }

        /// <summary>
        /// 获取订单运费详情
        /// </summary>
        /// <param name="order">订单</param>
        /// <returns>运费详情</returns>
        /// <remarks>2013-11-14 吴文强 创建</remarks>
        public static SoOrderFreight GetFreight(this SoOrder order)
        {
            var orderFreight = new SoOrderFreight();
            orderFreight.FreightSysNo = order.DeliveryTypeSysNo;
            orderFreight.FreightAmount = order.FreightAmount;
            orderFreight.FreightDiscountAmount = order.FreightDiscountAmount;
            orderFreight.FreightChangeAmount = order.FreightChangeAmount;

            //运费
            var freightAmount = order.FreightAmount - order.FreightDiscountAmount + order.FreightChangeAmount;

            //使用会员币时，先抵消运费
            orderFreight.CoinDeduction = (order.CoinPay > freightAmount ? freightAmount : order.CoinPay);

            //实际需要支付的运费
            orderFreight.RealFreightAmount = freightAmount - orderFreight.CoinDeduction;

            return orderFreight;
        }

        /// <summary>
        /// 根据订单来源获取订单使用的促销平台
        /// </summary>
        /// <param name="order">订单</param>
        /// <returns></returns>
        /// <remarks>2014-01-08 吴文强 创建</remarks>
        public static PromotionStatus.促销使用平台 GetPromotionPlatformType(this SoOrder order)
        {
            switch (order.OrderSource)
            {
                case (int)OrderStatus.销售单来源.手机商城:
                    return PromotionStatus.促销使用平台.手机商城;
                case (int)OrderStatus.销售单来源.门店下单:
                    return PromotionStatus.促销使用平台.门店;
                case (int)OrderStatus.销售单来源.业务员下单:
                    return PromotionStatus.促销使用平台.物流App;
                case (int)OrderStatus.销售单来源.分销商升舱:
                    return new PromotionStatus.促销使用平台();
            }
            return PromotionStatus.促销使用平台.PC商城;
        }
    }
}

