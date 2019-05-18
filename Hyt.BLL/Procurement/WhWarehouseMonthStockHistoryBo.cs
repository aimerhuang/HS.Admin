using Hyt.DataAccess.Procurement;
using Hyt.Model;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Procurement
{
    public class WhWarehouseMonthStockHistoryBo : BOBase<WhWarehouseMonthStockHistoryBo>
    {
        public  void InsertOrUpdateStockHistory(int year, int month, int day, int warehouseSysNo)
        {
            IWhWarehouseMonthStockHistoryDao.Instance.InsertOrUpdateStockHistory(year, month, day, warehouseSysNo);
        }

        public int InsertEntity(Model.Procurement.WhWarehouseMonthStockHistory history)
        {
            return IWhWarehouseMonthStockHistoryDao.Instance.InsertEntity(history);
        }

        public  void UpdateEntity(Model.Procurement.WhWarehouseMonthStockHistory history)
        {
            IWhWarehouseMonthStockHistoryDao.Instance.UpdateEntity(history);
        }

        public  List<Model.Procurement.WhWarehouseMonthStockHistory> GetWhWarehouseMonthStockHistory(int year, int month, int warehouseSysNo)
        {
            return IWhWarehouseMonthStockHistoryDao.Instance.GetWhWarehouseMonthStockHistory(year, month, warehouseSysNo);
        }

        public  void CheckAndUpdataMonthArea(DateTime starTime, DateTime endTime, int warehouseSysNo)
        {
            IWhWarehouseMonthStockHistoryDao.Instance.CheckAndUpdataMonthArea(starTime, endTime, warehouseSysNo);
        }

        public void GetWhWarehouseMonthStockHistoryPager(ref Model.Pager<Model.Procurement.CBWhWarehouseMonthStockHistory> pager)
        {
            IWhWarehouseMonthStockHistoryDao.Instance.GetWhWarehouseMonthStockHistoryPager(ref pager);
        }

        public void GetWhWarehousMouthSaleHistoryPager(ref Model.Pager<Model.Procurement.CBWhWarehouseMonthStockHistory> pager)
        {
            IWhWarehouseMonthStockHistoryDao.Instance.GetWhWarehousMouthSaleHistoryPager(ref pager);
        }

        public  void GetPaymentListDataPager(
          ref Model.Pager<CBFnReceiptVoucher> pager, int? PaymentTypeSysNo, int? WarehouseSysNo, DateTime? startTime, DateTime? endTime,
            string sysNoList)
        {
            IWhWarehouseMonthStockHistoryDao.Instance.GetPaymentListDataPager(ref pager, PaymentTypeSysNo, WarehouseSysNo, startTime, endTime, sysNoList);
        }
    }
}
