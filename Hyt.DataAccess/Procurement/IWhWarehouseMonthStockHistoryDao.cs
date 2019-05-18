using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Procurement;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Procurement
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class IWhWarehouseMonthStockHistoryDao : DaoBase<IWhWarehouseMonthStockHistoryDao>
    {
        public abstract void InsertOrUpdateStockHistory(
            int year, 
            int month, 
            int day, 
            int warehouseSysNo);
        public abstract int InsertEntity(WhWarehouseMonthStockHistory history);
        
        public abstract void UpdateEntity(WhWarehouseMonthStockHistory history);
        public abstract List<Model.Procurement.WhWarehouseMonthStockHistory> GetWhWarehouseMonthStockHistory(
            int year, 
            int month, 
            int warehouseSysNo);
        public abstract void CheckAndUpdataMonthArea(
            DateTime starTime, 
            DateTime endTime, 
            int warehouseSysNo);
        public abstract void GetWhWarehouseMonthStockHistoryPager(ref Model.Pager<CBWhWarehouseMonthStockHistory> pager);

        public abstract void GetWhWarehousMouthSaleHistoryPager(ref Model.Pager<CBWhWarehouseMonthStockHistory> pager);

        public abstract void GetPaymentListDataPager(
          ref Model.Pager<CBFnReceiptVoucher> pager, int? PaymentTypeSysNo, int? WarehouseSysNo, DateTime? startTime, DateTime? endTime,
            string sysNoList);
    }
}
