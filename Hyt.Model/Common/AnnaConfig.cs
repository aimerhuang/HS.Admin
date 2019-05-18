using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    public class AnnaConfig : ConfigBase
    {
        public string OnNumber { get; set; }
        public string WhNumber { get; set; }
        public string secretKey { get; set; }
        public string SenderUser { get; set; }
        public string SenderIdNumber { get; set; }
        public string supplierCode { get; set; }
        public string supplierName { get; set; }
        public string URLPath { get; set; }
        public string CourierCode { get; set; }

        public string PaymentCode { get; set; }
    }
}
