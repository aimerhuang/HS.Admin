using Hyt.DataAccess.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    public class DsPosFrontConfigDaoImpl : IDsPosFrontConfigDao
    {

        public override List<Model.Pos.DsPosFrontConfig> GetFrontConfigList()
        {
            string sql = "select * from DsPosFrontConfig ";
            return Context.Sql(sql).QueryMany<Model.Pos.DsPosFrontConfig>();
        }
    }
}
