using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transport
{
    public class DsWhCustomerMessage
    {
        public int SysNo { get; set; }
        public int GMSysNo { get; set; }
        public DateTime CurrentTime { get; set; }
        public string Message { get; set; }
    }
}
