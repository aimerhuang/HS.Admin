using Hyt.DataAccess.Base;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{
    public abstract class IDsPosOrderDao : DaoBase<IDsPosOrderDao>
    {
        public abstract int Insert(DsPosOrder order);
        public abstract void Update(DsPosOrder order);

        public abstract int InsertItem(DsPosOrderItem item);
        public abstract void UpdateItem(DsPosOrderItem item);

        public abstract DsPosOrder GetEntity(int sysNo);
        public abstract List<DsPosOrderItem> GetEntityItems(int pSysNo);
        public abstract List<DsPosOrder> GetList(int dsSysNo);

        public abstract List<DBDsPosOrder> GetPosOrderList(int posSysNo, DateTime stratday, DateTime endday, int DsSysNo);

        public abstract void DeleteDsPosOrder(int SysNo);

        public abstract void GetPosOrderListPagerByDsSysNo(ref Model.Pager<DBDsPosOrder> returnValue);

        public abstract DsPosOrder GetEntityBySellNumber(int dsSysNo, string sellNumber);

        public abstract List<DsPosOrderItem> GetEntityItems(List<int> soSysNoList);

        public abstract List<DBDsPosOrder> GetDBModList(int dsSysNo, DateTime? BeginDate, DateTime? EndDate);

        public abstract List<DsPosOrder> GetEntityBySellNumbers(int dsSysNo, List<string> orderNos);

        public abstract List<DsPosOrder> GetRepeatOrderList();

        public abstract void DeleteRepeatBySysNo(int SysNo);

        public abstract List<DBDsPosOrderItemData> GetPosOrderListByNoBindSale();

        public abstract List<WareDsPosOrder> GetPosOrderListByDate(DateTime dateTime);
    }
}
