using Hyt.DataAccess.Warehouse;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Warehouse
{
    public class ERPStockProductDaoImpl : IERPStockProductDao
    {
        public override int InsertMod(Model.Generated.ERPStockProduct mod)
        {
            return Context.Insert("ERPStockProduct", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateMod(Model.Generated.ERPStockProduct mod)
        {
             Context.Update("ERPStockProduct", mod).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }

        public override void DeleteMod(int SysNo)
        {
            string sql = " delete from  ERPStockProduct where SysNo='"+SysNo+"' ";
            Context.Sql(sql).Execute();
        }

        public override Model.Generated.ERPStockProduct GetMod(int SysNo)
        {
            string sql = " select * from ERPStockProduct where SysNo = '" + SysNo + "' ";
            return Context.Sql(sql).QuerySingle<ERPStockProduct>();
        }

        public override Model.Generated.ERPStockProduct GetMod(int ProductSysNo, int StockSysNo)
        {
            string sql = " select * from ERPStockProduct where ProductSysNo = '" + ProductSysNo + "' and  StockSysNo='"+StockSysNo+"' ";
            return Context.Sql(sql).QuerySingle<ERPStockProduct>();
        }

        public override List<Model.Generated.ERPStockProduct> GetWarehouseProductList(int? wareSysNo)
        {
            string sql = "";
            if(wareSysNo!=null)
            {
                sql = " select * from ERPStockProduct where  StockSysNo='" + wareSysNo.Value + "' ";
            }
            else
            {
                sql = " select * from ERPStockProduct  ";
            }
            return Context.Sql(sql).QueryMany<ERPStockProduct>();
        }

        public override List<ERPStockProduct> GetWarehouseProductListByProductSysNo(List<int> proList)
        {
            string sql = " select * from ERPStockProduct where ProductSysNo in (" + string.Join(",", proList.ToArray()) + ") ";
            return Context.Sql(sql).QueryMany<ERPStockProduct>();
        }
    }
}
