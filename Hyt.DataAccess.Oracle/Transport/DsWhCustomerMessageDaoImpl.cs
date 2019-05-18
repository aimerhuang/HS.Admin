using Hyt.DataAccess.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Transport
{
    public class DsWhCustomerMessageDaoImpl : IDsWhCustomerMessageDao
    {
        public override int InsertMod(Model.Transport.DsWhCustomerMessage mod)
        {
            return Context.Insert("DsWhCustomerMessage", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateMod(Model.Transport.DsWhCustomerMessage mod)
        {
            Context.Update("DsWhCustomerMessage", mod).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }

        public override List<Model.Transport.DsWhCustomerMessage> GetDsWhCustomerMessageByGMSysNo(int GMSysNo)
        {
            string sql = "select * from DsWhCustomerMessage where  GMSysNo ='" + GMSysNo + "' ";
            return Context.Sql(sql).QueryMany<Hyt.Model.Transport.DsWhCustomerMessage>();
        }
    }
}
