using Hyt.DataAccess.Base;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{   
    /// <summary>
    /// 退货订单的操作接口
    /// </summary>
    /// <remarks>2016-02-24</remarks>
    public abstract class IDsPosReturnOrderDao : DaoBase<IDsPosReturnOrderDao>
    {
        public abstract int Insert(DsPosReturnOrder rOrder);
        public abstract int InsertItem(DsPosReturnOrderItem rOrderItem);

        public abstract void Update(DsPosReturnOrder rOrder);
        public abstract void UpdateItem(DsPosReturnOrderItem rOrderItem);

        public abstract DsPosReturnOrder GetEntity(int SysNo);
        public abstract List<CBDsPosReturnOrderItem> GetEntityItemList(int pSysNo);

        public abstract List<DsPosReturnOrder> GetPosReturnOrderList(int posSysNo, DateTime startday, DateTime endday, int DsSysNo);

        public abstract void GetPosReturnOrderListPagerByDsSysNo(ref Model.Pager<CBDsPosReturnOrder> pager);

        public abstract CBDsPosReturnOrder GetCBPosReturnOrder(int SysNo);

        public abstract List<DsPosReturnOrder> GetAllReturnOrder();
        public abstract List<DsPosReturnOrderItem> GetAllReturnOrderItem();
    }
}
