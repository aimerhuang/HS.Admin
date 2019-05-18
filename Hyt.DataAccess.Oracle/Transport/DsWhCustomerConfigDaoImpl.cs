using Hyt.DataAccess.Transport;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Transport
{
    public class DsWhCustomerConfigDaoImpl : IDsWhCustomerConfigDao
    {
        public override int InsertMod(Model.Transport.DsWhCustomerConfig mod)
        {
            return Context.Insert("DsWhCustomerConfig", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateMod(Model.Transport.DsWhCustomerConfig mod)
        {
            Context.Update("DsWhCustomerConfig", mod).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }

        public override Model.Transport.DsWhCustomerConfig GetDsWhCustomerConfig(int SysNo)
        {
            string sql = "select * from DsWhCustomerConfig where SysNo ='" + SysNo + "' ";
            return Context.Sql(sql).QuerySingle<DsWhCustomerConfig>();
        }

        public override List<Model.Transport.DsWhCustomerConfig> GetDsWhCustomerList()
        {
            string sql = "select * from DsWhCustomerConfig ";
            return Context.Sql(sql).QueryMany<DsWhCustomerConfig>();
        }
    }
}
