using Hyt.DataAccess.Base;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{
    /// <summary>
    /// 条码支付历史记录
    /// </summary>
    public abstract class IDsPosBarcodePayLogDao : DaoBase<IDsPosBarcodePayLogDao>
    {

        public abstract int InnerDsPosBarcodePayLog(DsPosBarcodePayLog Mod);
        public abstract void UpdateDsPosBarcodePayLog(DsPosBarcodePayLog Mod);
        public abstract void DeleteDsPosBarcodePayLog(int SysNo);

        public abstract void DsPosBarcodePayLogPager(ref Model.Pager<DsPosBarcodePayLog> pager);
    }
}
