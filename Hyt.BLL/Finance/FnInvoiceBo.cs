using System.Collections.Generic;
using Hyt.DataAccess.Order;
using Hyt.Infrastructure.Memory;
using Hyt.Model;

namespace Hyt.BLL.Finance
{
    /// <summary>
    /// 发票相关
    /// </summary>
    /// <remarks>2013-07-08 朱成果 创建</remarks>
    public class FnInvoiceBo : BOBase<FnInvoiceBo>
    {
        #region 发票类型
        /// <summary>
        /// 获取发票类型列表
        /// </summary>
        /// <returns>发票类型列表</returns>
        /// <remarks>2013-07-10 朱成果 创建</remarks>
        public IList<Model.FnInvoiceType> GetFnInvoiceTypeList()
        {
            return IFnInvoiceDao.Instance.GetFnInvoiceTypeList();
        }

        /// <summary>
        /// 获取发票类型信息
        /// </summary>
        /// <param name="sysNo">发票类型编号</param>
        /// <returns>发票类型实体</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public FnInvoiceType GetFnInvoiceType(int sysNo)
        {
            return IFnInvoiceDao.Instance.GetFnInvoiceType(sysNo);
        }
        #endregion

        #region 发票

        /// <summary>
        /// 根据订单号获取订单发票信息
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns>发票实体</returns>
        /// <remarks>2013-06-21 周唐炬  创建</remarks>
        public FnInvoice GetFnInvoiceByOrderId(int orderId)
        {
            return IFnInvoiceDao.Instance.GetFnInvoiceByOrderID(orderId);
        }

        /// <summary>
        /// 获取订单有效发票(已开票的发票)
        /// </summary>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <returns>发票</returns>
        /// <remarks>2013-10-29 吴文强 创建</remarks>
        public FnInvoice GetOrderValidInvoice(int orderSysNo)
        {
            return IFnInvoiceDao.Instance.GetOrderValidInvoice(orderSysNo);
        }

        /// <summary>
        /// 计算发票金额(退换货商品的发票金额)
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <param name="refundProductAmount">退换货商品金额</param>
        /// <returns>发票扣款金额</returns>
        /// <remarks>2013-10-29 吴文强 创建</remarks>
        public decimal DeductedInvoiceAmount(int orderSysNo, decimal refundProductAmount)
        {
            decimal deductedInvoiceAmount = 0;
            var orderInvoice = GetOrderValidInvoice(orderSysNo);
            if (orderInvoice != null)
            {
                var invoiceType = GetFnInvoiceType(orderInvoice.InvoiceTypeSysNo);
                if (invoiceType != null)
                {
                    deductedInvoiceAmount = refundProductAmount * invoiceType.Percentage;
                }
            }
            return deductedInvoiceAmount;
        }

        /// <summary>
        /// 根据系统编号获取发票信息
        /// </summary>
        /// <param name="sysNo">发票编号</param>
        /// <returns></returns>
        /// <remarks>2013-06-21 朱成果 创建</remarks>
        /// <remarks>2014-05-14 余勇 修改 通过缓存取数据</remarks>
        public Model.FnInvoice GetFnInvoice(int sysNo)
        {
            return MemoryProvider.Default.Get(string.Format(KeyConstant.FnInvoice, sysNo), () => Hyt.DataAccess.Order.IFnInvoiceDao.Instance.GetFnInvoice(sysNo));
            //return Hyt.DataAccess.Order.IFnInvoiceDao.Instance.GetFnInvoice(sysNo);

        }
        #endregion
    }
}
