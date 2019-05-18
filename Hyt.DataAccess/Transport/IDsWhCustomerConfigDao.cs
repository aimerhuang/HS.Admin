using Hyt.DataAccess.Base;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Transport
{
    public abstract class IDsWhCustomerConfigDao : DaoBase<IDsWhCustomerConfigDao>
    {
        public abstract int InsertMod(DsWhCustomerConfig mod);

        public abstract void UpdateMod(DsWhCustomerConfig mod);

        public abstract DsWhCustomerConfig GetDsWhCustomerConfig(int SysNo);

        public abstract List<DsWhCustomerConfig> GetDsWhCustomerList();
    }
}
