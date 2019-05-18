using Hyt.DataAccess.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    public class SyWSOrderToClientJobDaoImpl : ISyWSOrderToClientJobDao
    {
        public override int InnerMod(Model.Pos.SyWSOrderToClientJob mod)
        {
            return Context.Insert("SyWSOrderToClientJob", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override int UpdateMod(Model.Pos.SyWSOrderToClientJob mod)
        {
            return Context.Update("SyWSOrderToClientJob", mod).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }

        public override List<Model.SoOrder> GetNotPosthOrderDataToClientList(int dayValue)
        {
            string sql = " select SoOrder.* from SoOrder left join SyWSOrderToClientJob on SyWSOrderToClientJob.OrderSysNo=SoOrder.SysNo ";
            sql += " where  SoOrder.CreateDate>='" + DateTime.Now.AddDays(-1 * dayValue).ToString("yyyy-MM-dd") + " 00:00:00' and SyWSOrderToClientJob.sysNo is null ";
            return Context.Sql(sql).QueryMany<Hyt.Model.SoOrder>();
        }

        public override List<Model.Pos.SyWSOrderToClientJob> GetPoshDataToClientList(int? DsSysNo)
        {
            string sql = "";
            if (DsSysNo!=null)
            {
                sql = "  select  * from SyWSOrderToClientJob where DsSysNo='" + DsSysNo.Value + "' ";
            }
            else
            {
                sql = "  select  * from SyWSOrderToClientJob  ";
            }
            return Context.Sql(sql).QueryMany<Hyt.Model.Pos.SyWSOrderToClientJob>();
        }
    }
}
