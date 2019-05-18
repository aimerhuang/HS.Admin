using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    public class Customs3Config
    {
        public string MessageType { get; set; }
        public string SenderID { get; set; }
        public string Version { get; set; }
        public string Receiver { get; set; }
        public string EbcCode { get; set; }
        public string EbcName { get; set; }
        public string WebDomain { get; set; }
        public string EbcFTPName { get; set; }
        public string EbcFtpUrl { get; set; }
        public string EbcFtpPassword { get; set; }
        public string DxpId { get; set; }

        public string CustomsCode { get; set; }

        public string ICPBCCode { get; set; }

        public string ICPBBCCode { get; set; }
    }
}
