using Hyt.DataAccess.Base;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Warehouse
{
    public abstract class IERPStockProductDao : DaoBase<IERPStockProductDao>
    {
        public abstract int InsertMod(ERPStockProduct mod);
        public abstract void UpdateMod(ERPStockProduct mod);
        public abstract void DeleteMod(int SysNo);
        public abstract ERPStockProduct GetMod(int SysNo);
        public abstract ERPStockProduct GetMod(int ProductSysNo,int StockSysNo);
        public abstract List<ERPStockProduct> GetWarehouseProductList(int? wareSysNo);

        public abstract List<ERPStockProduct> GetWarehouseProductListByProductSysNo(List<int> proList);
    }
}
