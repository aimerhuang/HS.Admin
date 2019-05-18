using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transport
{
    public class CBDsWhLogisticsNumber : DsWhLogisticsNumber
    {
        public int num { get; set; }
    }
    /// <summary>
    /// 物流编码档案集合
    /// </summary>
    public class DsWhLogisticsNumber
    {
        public int SysNo { get; set; }
        public int LgSysNo { get; set; }
        public string LgCode { get; set; }
        public string LgNumber { get; set; }
        public int LgUsed { get; set; }
        public string CusCode { get; set; }
        public string UsedTime { get; set; }
    }
}
