using Hyt.DataAccess.Base;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{
    public abstract class IDsPosFrontDealerConfigDao : DaoBase<IDsPosFrontDealerConfigDao>
    {
        public abstract int InsertMod(DsPosFrontDealerConfig mod);

        public abstract void UpdateMod(DsPosFrontDealerConfig mod);

        public abstract void DeleteMod(int DsSysNo);

        public abstract List<DsPosFrontDealerConfig> GetModelList(int DsSysNo);
    }
}
