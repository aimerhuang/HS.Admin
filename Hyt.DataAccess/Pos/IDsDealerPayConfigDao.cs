using Hyt.DataAccess.Base;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{
    public abstract class IDsDealerPayConfigDao : DaoBase<IDsDealerPayConfigDao>
    {
        public abstract int InnerMod(DsDealerPayConfig mod);
        public abstract void UpdateMod(DsDealerPayConfig mod);
        public abstract DsDealerPayConfig GetMod(int DsSysNo, string type);
    }
}
