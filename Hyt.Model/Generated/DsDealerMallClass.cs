using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Generated
{
    public class DsDealerMallClass
    {
        public int SysNo { get; set; }
        public string DmcName { get; set; }
        public string DmcTemplatePath { get; set; }
        public string DmcProductClass { get; set; }
        /// <summary>
        /// 商品展示类型
        /// </summary>
        public string DmcShowProClass { get; set; }
        public DateTime DmcDate { get; set; }
        public int DealerSysNo { get; set; }
        public int IsOpen { get; set; }
    }
}
