using Hyt.DataAccess.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Warehouse
{
    public class ERPStockProductBo : BOBase<ERPStockProductBo>
    {
        public  int InsertMod(Model.Generated.ERPStockProduct mod)
        {
            return IERPStockProductDao.Instance.InsertMod(mod);
        }

        public  void UpdateMod(Model.Generated.ERPStockProduct mod)
        {
            IERPStockProductDao.Instance.UpdateMod(mod);
        }

        public  void DeleteMod(int SysNo)
        {
            IERPStockProductDao.Instance.DeleteMod(SysNo);
        }

        public  Model.Generated.ERPStockProduct GetMod(int SysNo)
        {
            return IERPStockProductDao.Instance.GetMod(SysNo);
        }

        public  Model.Generated.ERPStockProduct GetMod(int ProductSysNo, int StockSysNo)
        {
            return IERPStockProductDao.Instance.GetMod(ProductSysNo, StockSysNo);
        }

        public  List<Model.Generated.ERPStockProduct> GetWarehouseProductList(int? wareSysNo)
        {
            return IERPStockProductDao.Instance.GetWarehouseProductList(wareSysNo);
        }

        public List<Model.Generated.ERPStockProduct> GetWarehouseProductListByProductSysNo(List<int> proList)
        {
            return IERPStockProductDao.Instance.GetWarehouseProductListByProductSysNo(proList);
        }
    }
}
