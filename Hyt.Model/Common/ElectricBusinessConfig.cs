using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    public class ElectricBusinessConfig
    {
        /// <summary>
        /// 海关编码
        /// </summary>
        public string CusCode { get; set; }
        /// <summary>
        /// 海关名称
        /// </summary>
        public string CusName { get; set; }
        /// <summary>
        /// icp编码
        /// </summary>
        public string ICPCode { get; set; }
        /// <summary>
        /// icp名称
        /// </summary>
        public string ICPName { get; set; }
        /// <summary>
        /// 支付企业编码
        /// </summary>
        public string PayCode { get; set; }
        /// <summary>
        /// 支付企业名称
        /// </summary>
        public string PayName { get; set; }
    }
}
