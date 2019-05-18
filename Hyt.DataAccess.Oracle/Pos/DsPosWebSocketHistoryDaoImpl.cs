using Hyt.DataAccess.Pos;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    public class DsPosWebSocketHistoryDaoImpl : IDsPosWebSocketHistoryDao
    {
        public override int Inser(Model.Pos.DsPosWebSocketHistory mod)
        {
            return Context.Insert("DsPosWebSocketHistory", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void Update(Model.Pos.DsPosWebSocketHistory mod)
        {
            Context.Update("DsPosWebSocketHistory", mod).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }

        public override DsPosWebSocketHistory GetModel(int SysNo)
        {
            string sql="select * from DsPosWebSocketHistory where SysNo='"+SysNo+"'";
            return Context.Sql(sql).QuerySingle<DsPosWebSocketHistory>();
        }

        public override List<Model.Pos.DsPosWebSocketHistory> GetList(int DsSysNo)
        {
            string sql = "select * from DsPosWebSocketHistory where DsSysNo='" + DsSysNo + "'";
            return Context.Sql(sql).QueryMany<DsPosWebSocketHistory>();
        }
    }
}
