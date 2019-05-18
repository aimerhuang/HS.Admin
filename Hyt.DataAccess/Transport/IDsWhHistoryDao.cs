using Hyt.DataAccess.Base;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Transport
{
    public abstract class IDsWhHistoryDao : DaoBase<IDsWhHistoryDao>
    {
        public abstract int insertMod(DsWhHistory history);
        public abstract List<CBDsWhHistory> GetHistoryListByCourierNumber(string Code);

        public abstract CBDsWhHistory GetEntityBySysNo(int SysNo);
        public abstract void UpdateMod(DsWhHistory entity);
    }
}
