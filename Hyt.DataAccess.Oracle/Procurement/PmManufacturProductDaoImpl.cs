using Hyt.DataAccess.Procurement;
using Hyt.Model.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Procurement
{
    public class PmManufacturProductDaoImpl : IPmManufacturProductDao
    {
        public override int InsertData(Model.Procurement.PmManufacturProduct mod)
        {
            return Context.Insert("PmManufacturProduct", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateData(Model.Procurement.PmManufacturProduct mod)
        {
            Context.Update("PmManufacturProduct", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }

        public override void DeleteData(int ProSysNo)
        {
            string sql = " delete from PmManufacturProduct where ProductSysNo=" + ProSysNo + " ";
            Context.Sql(sql).Execute();
        }

        public override void DeleteData(int ProSysNo, int PmSysNo)
        {
            string sql = " delete from PmManufacturProduct where ProductSysNo=" + ProSysNo + " and PmSysNo=" + PmSysNo + " ";
            Context.Sql(sql).Execute();
        }

        public override Model.Procurement.CBPmManufacturProduct GetManufacturProduct(int ProSysNo, int PmSysNo)
        {
            string sql = " select PmManufacturProduct.*,PmManufacturer.FName from PmManufacturProduct inner join PmManufacturer on PmManufacturProduct.PmSysNo=PmManufacturer.SysNo where ProductSysNo=" + ProSysNo + " and PmSysNo=" + PmSysNo + " ";
            return Context.Sql(sql).QuerySingle<CBPmManufacturProduct>();
        }

        public override List<Model.Procurement.CBPmManufacturProduct> GetManufacturProductByList(int ProSysNo)
        {
            string sql = " select PmManufacturProduct.*,PmManufacturer.FName from PmManufacturProduct inner join PmManufacturer on PmManufacturProduct.PmSysNo=PmManufacturer.SysNo where ProductSysNo=" + ProSysNo  + " ";
            return Context.Sql(sql).QueryMany<CBPmManufacturProduct>();
        }
    }
}
