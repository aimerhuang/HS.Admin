using Hyt.DataAccess.Purchase;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Purchase
{
    /// <summary>
    /// 采购退货单详情
    /// </summary>
    /// <remarks>2016-6-17 王耀发 创建</remarks>
    public class PrPurchaseReturnDetailsBo : BOBase<PrPurchaseReturnDetailsBo>
    {
        public int Delete(string sysNos)
        {
            return IPrPurchaseReturnDetailsDao.Instance.Delete(sysNos);
        }
        /// <summary>
        /// 获取采购退货单的所有有采购商品
        /// </summary>
        /// <param name="PurchaseReturnSysNo">采购退货单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-17 王耀发 创建</remarks>
        public IList<PrPurchaseReturnDetails> GetPurchaseReturnDetailsList(int PurchaseReturnSysNo)
        {
            return IPrPurchaseReturnDetailsDao.Instance.GetPurchaseReturnDetailsList(PurchaseReturnSysNo);
        }
        /// <summary>
        /// 更新采购退货单详情已出库数
        /// </summary>
        /// <param name="purchaseReturnSysNo">采购退货单系统编号</param>
        /// <param name="productSysNo">产品系统编号</param>
        /// <param name="outQuantity">已出库数</param>
        /// <returns></returns>
        /// <remarks>2016-6-21 王耀发 创建</remarks>
        public bool UpdateOutQuantity(int purchaseReturnSysNo, int productSysNo, int outQuantity)
        {
            return IPrPurchaseReturnDetailsDao.Instance.UpdateOutQuantity(purchaseReturnSysNo, productSysNo, outQuantity);
        }
    }
}
