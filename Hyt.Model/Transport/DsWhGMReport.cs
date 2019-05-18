using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transport
{
    public class DsWhGMReport
    {
        public int AllNumber { get; set; }
        public int AuditNumber { get; set; }
        public int NotPass { get; set; }
        public int WaitCalculation { get; set; }
        public int WaitPay { get; set; }
        public int WaitOutStock { get; set; }
        public int HaveOutStock { get; set; }
        public int HaveGet { get; set; }
    }
}
