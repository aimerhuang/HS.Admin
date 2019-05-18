using Hyt.DataAccess.Base;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{
    public abstract class IDsPosFrontConfigDao : DaoBase<IDsPosFrontConfigDao>
    {
        public abstract List<DsPosFrontConfig> GetFrontConfigList();
    }
}
