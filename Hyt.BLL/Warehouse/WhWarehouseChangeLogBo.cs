using Hyt.DataAccess.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Warehouse
{
    public class WhWarehouseChangeLogBo : BOBase<WhWarehouseChangeLogBo>
    {
        public  int CreateMod(Model.Generated.WhWarehouseChangeLog log)
        {
            return IWhWarehouseChangeLogDao.Instance.CreateMod(log);
        }

        bool CheckWhWarehouseChangeLogTable(string table)
        {
           return IWhWarehouseChangeLogDao.Instance.CheckWhWarehouseChangeLogTable(table);
        }

        public  void UpdateMod(Model.Generated.WhWarehouseChangeLog log)
        {
            IWhWarehouseChangeLogDao.Instance.UpdateMod(log);
        }

        public  void DeleteMod(int SysNo)
        {
            IWhWarehouseChangeLogDao.Instance.DeleteMod(SysNo);
        }

        public  List<Model.Generated.WhWarehouseChangeLog> WarehouseLogList(int wareSysNo, int proSysNo)
        {
            return IWhWarehouseChangeLogDao.Instance.WarehouseLogList(wareSysNo, proSysNo);
        }

        public List<Model.Generated.WhWarehouseChangeLog> WarehouseLogList(int wareSysNo)
        {
            return IWhWarehouseChangeLogDao.Instance.WarehouseLogList(wareSysNo);
        }

        public List<Model.Generated.WhWarehouseChangeLog> WarehouseLogList(string OrderNo)
        {
            return IWhWarehouseChangeLogDao.Instance.WarehouseLogList(OrderNo);
        }

        public void UpdateWhInventoryWarehouseLog()
        {
            IWhWarehouseChangeLogDao.Instance.UpdateWhInventoryWarehouseLog();
        }
    }
}
