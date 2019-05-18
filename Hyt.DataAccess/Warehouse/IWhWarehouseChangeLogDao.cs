using Hyt.DataAccess.Base;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Warehouse
{
    /// <summary>
    /// 库存情况变化记录
    /// </summary>
    /// <remarks>2016-12-26 杨云奕 添加</remarks>
    public abstract class IWhWarehouseChangeLogDao : DaoBase<IWhWarehouseChangeLogDao>
    {
        public abstract int CreateMod(WhWarehouseChangeLog log);

        public abstract void UpdateMod(WhWarehouseChangeLog log);
        public abstract void DeleteMod(int SysNo);
        public abstract List<WhWarehouseChangeLog> WarehouseLogList(int wareSysNo, int proSysNo);

        public abstract bool CheckWhWarehouseChangeLogTable(string table);
        public abstract void CreateWhWarehouseChangeLogTable(string table);

        public abstract List<Model.Generated.WhWarehouseChangeLog> WarehouseLogList(int wareSysNo);

        public abstract List<WhWarehouseChangeLog> WarehouseLogList(string OrderNo);

        public abstract void UpdateWhInventoryWarehouseLog();
    }
}
