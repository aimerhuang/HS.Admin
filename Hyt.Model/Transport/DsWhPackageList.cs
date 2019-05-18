using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transport
{
    public class CBDsWhPackageList : DsWhPackageList
    {
        public string CustomerCode { get; set; }
        public string ScanDatetime { get; set; }
        public string ScannedName { get; set; }
        public string ConfirmedName { get; set; }
        public string CityInfo { get; set; }

        public decimal WeightValue { get; set; }
    }
    public class DsWhPackageList
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 包裹父id
        /// </summary>
        public int PSysNo { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string CourierNumber { get; set; }
        /// <summary>
        /// 扫描时间
        /// </summary>
        public DateTime ScannedDatetime { get; set; }
        /// <summary>
        /// 扫描人员
        /// </summary>
        public int ScannedBy { get; set; }
        /// <summary>
        /// 修改人员
        /// </summary>
        public int ConfirmedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ConfirmedDatetime { get; set; }
    }
}
