using Hyt.DataAccess.Base;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Transport
{
    public abstract class IDsWhCustomerMessageDao : DaoBase<IDsWhCustomerMessageDao>
    {
        public abstract int InsertMod(DsWhCustomerMessage mod);
        public abstract void UpdateMod(DsWhCustomerMessage mod);
        public abstract List<DsWhCustomerMessage> GetDsWhCustomerMessageByGMSysNo(int GMSysNo);
    }
}
