using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    /// <summary>
    /// 付款定义
    /// </summary>
    public class DsDealerPayConfig
    {
        public int SysNo { get; set; }
        public string PayType { get; set; }
        public string MacID { get; set; }
        public string AppID { get; set; }
        public string SecKey { get; set; }
        public int DsSysNo { get; set; }
    }
}
