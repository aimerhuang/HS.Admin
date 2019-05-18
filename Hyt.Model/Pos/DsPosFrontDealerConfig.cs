using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    /// <summary>
    /// 收银员数据绑定
    /// </summary>
    public class DsPosFrontDealerConfig
    {
        public int SysNo { get; set; }
        public int DsSysNo { get; set; }
        public string ConfigCode { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
    }
}
