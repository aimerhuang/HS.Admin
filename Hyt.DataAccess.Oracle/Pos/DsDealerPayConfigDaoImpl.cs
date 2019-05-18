using Hyt.DataAccess.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    public class DsDealerPayConfigDaoImpl : IDsDealerPayConfigDao
    {
        public override int InnerMod(Model.Pos.DsDealerPayConfig mod)
        {
            throw new NotImplementedException();
        }

        public override void UpdateMod(Model.Pos.DsDealerPayConfig mod)
        {
            throw new NotImplementedException();
        }

        public override Model.Pos.DsDealerPayConfig GetMod(int DsSysNo, string type)
        {
            string sql = " select * from DsDealerPayConfig where DsSysNo='" + DsSysNo + "' and PayType='" + type + "' ";
            return Context.Sql(sql).QuerySingle<Hyt.Model.Pos.DsDealerPayConfig>();
        }
    }
}
