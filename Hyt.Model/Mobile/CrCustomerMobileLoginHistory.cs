using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Mobile
{
    public class CrCustomerMobileLoginHistory
    {
        public int SysNo { get; set; }
        public int CustomerMobileSysNo { get; set; }
        public DateTime CustomerLoginTime { get; set; }
        public string CustomerLoginKey { get; set; }
        public string CustomerLoginDevice { get; set; }
        public string CustomerLoginDeviceCode { get; set; }
    }
}
