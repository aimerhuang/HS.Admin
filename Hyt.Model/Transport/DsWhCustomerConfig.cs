using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transport
{
    public class DsWhCustomerConfig
    {
        /// <summary>
        ///自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 状态编码
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// 客户信息
        /// </summary>
        public string CustomerMsg { get; set; }
    }
}
