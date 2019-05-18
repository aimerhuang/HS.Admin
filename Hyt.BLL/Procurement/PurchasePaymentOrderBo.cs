using Hyt.DataAccess.Procurement;
using Hyt.Model.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Procurement
{
    public class PurchasePaymentOrderBo : BOBase<PurchasePaymentOrderBo>
    {
        #region 采购申请单
        public int InsertEntity(Model.Procurement.FnPurchasePaymentOrder mod)
        {
            return IPurchasePaymentOrderDao.Instance.InsertEntity(mod);
        }

        public void UpdateEntity(Model.Procurement.FnPurchasePaymentOrder mod)
        {
            IPurchasePaymentOrderDao.Instance.UpdateEntity(mod);
        }

        public void DeleteEntity(int SysNo)
        {
            IPurchasePaymentOrderDao.Instance.DeleteEntity(SysNo);
        }

        public Model.Procurement.CBPurchasePaymentOrder GetEntity(int SysNo)
        {
            return IPurchasePaymentOrderDao.Instance.GetEntity(SysNo);
        }

        public Model.Procurement.FnPurchasePaymentOrder GetEntityBySysNo(int SysNo)
        {
            return IPurchasePaymentOrderDao.Instance.GetEntity(SysNo);
        }

        public void GetPmPointsOrderPager(ref Model.Pager<Model.Procurement.CBPurchasePaymentOrder> pager)
        {
            IPurchasePaymentOrderDao.Instance.GetPmPointsOrderPager(ref pager);
        }
        #endregion
        #region 采购申请单明细
        /// <summary>
        /// 添加采购申请单明细
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int InsertItem(FnPurchasePaymentOrderItem item)
        {
            return IPurchasePaymentOrderDao.Instance.InsertItem(item);
        }
        /// <summary>
        /// 更新采购单明细
        /// </summary>
        /// <param name="item"></param>
        public void UpdateItem(FnPurchasePaymentOrderItem item)
        {
            IPurchasePaymentOrderDao.Instance.UpdateItem(item);
        }
        /// <summary>
        /// 删除采购单明细
        /// </summary>
        /// <param name="SysNo"></param>
        public void DeleteItem(int SysNo)
        {
            IPurchasePaymentOrderDao.Instance.DeleteItem(SysNo);
        }
        /// <summary>
        /// 获取采购明细单明细
        /// </summary>
        /// <param name="PSysNo"></param>
        /// <returns></returns>
        public List<CBFnPurchasePaymentOrderItem> GetOrderItems(int PSysNo)
        {
            return IPurchasePaymentOrderDao.Instance.GetOrderItems(PSysNo);
        }
        #endregion

        public List<PmProcurementOrder> ProcurementOrderByNotInPurchase()
        {
            return IPurchasePaymentOrderDao.Instance.ProcurementOrderByNotInPurchase();
        }
    }
}
