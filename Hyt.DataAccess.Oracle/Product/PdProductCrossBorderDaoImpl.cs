using Hyt.DataAccess.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Product
{
    public class PdProductCrossBorderDaoImpl : IPdProductCrossBorderDao
    {
        public override int InserMod(Model.Generated.PdProductCrossBorder mod)
        {
            return Context.Insert("PdProductCrossBorder", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateMod(Model.Generated.PdProductCrossBorder mod)
        {
            Context.Update("PdProductCrossBorder", mod).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }

        public override List<Model.Generated.PdProductCrossBorder> GetProductListBySysNos(List<int> proSysNos)
        {
            string sql = "select * from PdProductCrossBorder where ProductSysNo in (" + string.Join(",",proSysNos.ToArray()) + ") ";
            return Context.Sql(sql).QueryMany<Hyt.Model.Generated.PdProductCrossBorder>();
        }

        public override Hyt.Model.Generated.PdProductCrossBorder GetProductByProductSysNo(int proSysNo)
        {
            string sql = "select * from PdProductCrossBorder where ProductSysNo = " + proSysNo + " ";
            return Context.Sql(sql).QuerySingle<Hyt.Model.Generated.PdProductCrossBorder>();
        }
    }
}
