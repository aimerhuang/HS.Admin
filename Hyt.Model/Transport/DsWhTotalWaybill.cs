using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transport
{
    public class CBDsWhTotalWaybill : DsWhTotalWaybill
    {
        public string DeliveryTypeName { get; set; }
        public List<DsWhTotalWaybillList> ModList { get; set; }
    }
    public class DsWhTotalWaybill
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public string StatusCode { get; set; }
        /// <summary>
        /// 状态吗
        /// </summary>
        public string ServiceType { get; set; }
        /// <summary>
        /// 总运单号
        /// </summary>
        public string MawbNumber { get; set; }
        /// <summary>
        /// 航班时间
        /// </summary>
        public DateTime FlightDate { get; set; }
        /// <summary>
        /// 航班编号
        /// </summary>
        public string FlightNumber { get; set; }
        /// <summary>
        /// 抵达时间
        /// </summary>
        public DateTime ArrivalDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Dest { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalNum { get; set; }
        /// <summary>
        /// 总重量
        /// </summary>
        public decimal TotalWeight { get; set; }
        /// <summary>
        /// 包裹扫入人员
        /// </summary>
        public int EntryBy { get; set; }
        /// <summary>
        /// 包裹扫入时间
        /// </summary>
        public DateTime EntryDate { get; set; }
        /// <summary>
        /// 客户编码
        /// </summary>
        public string CusCode { get; set; }
        /// <summary>
        /// 修改状态时间
        /// </summary>
        public DateTime StatusCodeTime { get; set; }
    }
}
