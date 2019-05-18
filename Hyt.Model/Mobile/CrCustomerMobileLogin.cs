using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Mobile
{
    public class CrCustomerMobileLogin
    {
        public int SysNo { get; set; }
        public int CustomerSysNo { get; set; }
        public string CustomerToken { get; set; }
        public DateTime LastLoginTime { get; set; }
        public string CustomerLoginKey { get; set; }
        public string CustomerLoginDevice { get; set; }
        public string CustomerLoginDeviceCode { get; set; }
    }
}
