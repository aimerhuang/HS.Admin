using Hyt.DataAccess.Base;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{
    public abstract class IDsTakeStockOrderDao : DaoBase<IDsTakeStockOrderDao>
    {
        public abstract int Insert(DsTakeStockOrder mod);
        public abstract void InsertItem(DsTakeStockItem mod);

        public abstract void GetTakeStockOrderListPagerByDsSysNo(ref Model.Pager<CBDsTakeStockOrder> pager);

        public abstract DsTakeStockOrder GetTakeStockOrder(int SysNo);

        public abstract List<CBDsTakeStockItem> GetTakeStockOrderItems(int SysNo);

        public abstract List<CBDsTakeStockOrder> GetTakeStockOrderList(int? dsSysNo, DateTime? datetime);

        public abstract List<DsTakeStockItem> GettTakeStockItems(List<int> SysNos);
    }
}
