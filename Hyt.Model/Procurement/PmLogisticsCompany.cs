using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Procurement
{
    /// <summary>
    /// 物流公司实体
    /// </summary>
    public class PmLogisticsCompany
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 物流公司名称
        /// </summary>
        public string LCName { get; set; }
        /// <summary>
        /// 物流公司类型
        /// </summary>
        public string LCType { get; set; }
        /// <summary>
        /// 物流公司备注
        /// </summary>
        public string LCDis { get; set; }

    }
}
