using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    public class DsPosWebSocketHistory
    {
        public int SysNo { get; set; }
        public int DsSysNo { get; set; }
        public string DsPosName { get; set; }
        public int OrderSysNo { get; set; }
        public string ParamText { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
