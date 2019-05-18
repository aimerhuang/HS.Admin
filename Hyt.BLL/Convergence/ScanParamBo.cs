using Hyt.DataAccess.Convergence;
using Hyt.Model.Convergence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Convergence
{
    public class ScanParamBo : BOBase<ScanParamBo>
    {
        public ScanParam GetScanParam(int OrderNo)
        {
            return ScanParamDao.Instance.GetScanParam(OrderNo);
        }
    }
}
