using Hyt.DataAccess.Base;
using Hyt.Model.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Procurement
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class IPmProcurementDao : DaoBase<IPmProcurementDao>
    {
        public abstract int CreatePmProcurementOrder(PmProcurementOrder order);
        public abstract int CreatePmProcurementOrderItem(PmProcurementOrderItem order);
        public abstract int CreatePmProcurementWebType(PmProcurementWebType webType);
        public abstract int CreatePmProcurementWebPrice(PmProcurementWebPrice webPrice);


        public abstract void UpdatePmProcurementOrder(PmProcurementOrder order);
        public abstract void UpdatePmProcurementOrderItem(PmProcurementOrderItem order);
        public abstract void UpdatePmProcurementWebType(PmProcurementWebType webType);
        public abstract void UpdatePmProcurementWebPrice(PmProcurementWebPrice webPrice);


        public abstract void UpdatePmProcurementOrderStatus(int SysNo, int Status, int UpdateBy);


        public abstract List<PmProcurementOrderItem> GetProcurementOrderItemList(int pSysNo);
        public abstract List<CBPmProcurementOrderItem> GetCBProcurementOrderItemList(int pSysNo);
        public abstract List<PmProcurementWebType> GetProcurementWebTypeList();
        public abstract List<CBPmProcurementWebPrice> GetProcurementWebPriceList(int[] itemSysNo);

        public abstract void GetProcurmentWebTypePager(Model.Pager<PmProcurementWebType> pager);
        public abstract void GetPmProcurementOrderPager(Model.Pager<CBPmProcurementOrder> pager);

        public abstract CBPmProcurementOrder GetCBPmProcurementOrder(int SysNo);

        public abstract IList<CBPmProcurementOrderItem> GetCBProcurementOrderItemListByProList(string proIdList, int cgSysNo);

        public abstract void UpdatePmProcurementOrderItemStatus(int itemSysNo, int value);

        public abstract CBPmProcurementOrder GetCBPmProcurementOrder(string pmNumber);



        public abstract List<CBPmProcurementOrderItem> GetPmProcurementOrderItem(int[] pSysNo);
    }
}
