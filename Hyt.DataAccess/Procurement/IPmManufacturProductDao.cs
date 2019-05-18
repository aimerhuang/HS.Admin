using Hyt.DataAccess.Base;
using Hyt.Model.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Procurement
{
    public abstract class IPmManufacturProductDao : DaoBase<IPmManufacturProductDao>
    {
        public abstract int InsertData(PmManufacturProduct mod);
        public abstract void UpdateData(PmManufacturProduct mod);
        public abstract void DeleteData(int ProSysNo);
        public abstract void DeleteData(int ProSysNo,int PmSysNo);

        public abstract CBPmManufacturProduct GetManufacturProduct(int ProSysNo, int PmSysNo);
        public abstract List<CBPmManufacturProduct> GetManufacturProductByList(int ProSysNo);
    }
}
