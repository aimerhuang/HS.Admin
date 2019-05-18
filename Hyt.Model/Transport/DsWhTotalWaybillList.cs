using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transport
{
    public class DsWhTotalWaybillList
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 航班父id
        /// </summary>
        public int PSysNo { get; set; }
        /// <summary>
        /// 包裹编号
        /// </summary>
        public string PackageNumber { get; set; }
        /// <summary>
        /// 包裹总数量
        /// </summary>
        public int TotalNum { get; set; }
        /// <summary>
        /// 包裹总重量
        /// </summary>
        public decimal TotalWeight { get; set; }
        /// <summary>
        /// 服务类型
        /// </summary>
        public string Dest { get; set; }
    }
}
