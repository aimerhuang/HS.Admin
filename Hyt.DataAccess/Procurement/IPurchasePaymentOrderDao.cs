using Hyt.DataAccess.Base;
using Hyt.Model.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Procurement
{
    public abstract class IPurchasePaymentOrderDao : DaoBase<IPurchasePaymentOrderDao>
    {
        public abstract int InsertEntity(FnPurchasePaymentOrder mod);
        public abstract void UpdateEntity(FnPurchasePaymentOrder mod);
        public abstract void DeleteEntity(int SysNo);
        public abstract CBPurchasePaymentOrder GetEntity(int SysNo);
        public abstract FnPurchasePaymentOrder GetEntityBySysNo(int SysNo);
        public abstract void GetPmPointsOrderPager(ref Model.Pager<CBPurchasePaymentOrder> pager);
        
        public abstract List<CBPmPointsOrderItem> GetPurPointsOrderItemData(string ManuText, int pmSysNo);

        #region 获取其采购单明细
        public abstract int InsertItem(FnPurchasePaymentOrderItem item);

        public abstract void UpdateItem(FnPurchasePaymentOrderItem item);

        public abstract List<CBFnPurchasePaymentOrderItem> GetOrderItems(int PSysNo);

        public abstract void DeleteItem(int SysNo);
        #endregion

        /// <summary>
        /// 获取未设置采购单的商品数据
        /// </summary>
        /// <returns></returns>
        public abstract List<PmProcurementOrder> ProcurementOrderByNotInPurchase();
    }
}
