using Hyt.DataAccess.Base;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Transport
{
    public abstract class IDsWhLogisticsNumberDao : DaoBase<IDsWhLogisticsNumberDao>
    {
        public abstract int InsertMod(DsWhLogisticsNumber mod);
        public abstract void UpdataMod(DsWhLogisticsNumber mod);
        public abstract DsWhLogisticsNumber GetLogisticsNumberByNotUsed(string type);
        public abstract List<DsWhLogisticsNumber> GetAllLogisticsNumberByServiceType(string type);


        public abstract List<DsWhLogisticsNumber> GetLogisticsNumberByNotUsed(Dictionary<string, int> dicType);

        public abstract List<CBDsWhLogisticsNumber> GetLogisticsNumberListByCodeList(List<string> listNumber);
    }
}
