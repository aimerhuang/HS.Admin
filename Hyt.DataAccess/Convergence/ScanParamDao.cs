using Hyt.DataAccess.Base;
using Hyt.Model.Convergence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Convergence
{
    public abstract class ScanParamDao : DaoBase<ScanParamDao>
    {
        public abstract ScanParam GetScanParam(int OrderNo);
    }
}
