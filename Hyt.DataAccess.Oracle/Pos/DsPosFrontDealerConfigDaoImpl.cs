using Hyt.DataAccess.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    public class DsPosFrontDealerConfigDaoImpl : IDsPosFrontDealerConfigDao
    {
        public override int InsertMod(Model.Pos.DsPosFrontDealerConfig mod)
        {
            return Context.Insert("DsPosFrontDealerConfig", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateMod(Model.Pos.DsPosFrontDealerConfig mod)
        {
            Context.Update("DsPosFrontDealerConfig", mod).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }

        public override void DeleteMod(int DsSysNo)
        {
            string sql = "delete from DsPosFrontDealerConfig where DsSysNo = '"+DsSysNo+"'";
            Context.Sql(sql).Execute();
        }

        public override List<Model.Pos.DsPosFrontDealerConfig> GetModelList(int DsSysNo)
        {
            string sql = "select * from DsPosFrontDealerConfig where DsSysNo = '" + DsSysNo + "'";
            return Context.Sql(sql).QueryMany<Hyt.Model.Pos.DsPosFrontDealerConfig>();
        }
    }
}
