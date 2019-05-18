using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transport
{
    public class CBDsWhPackage:DsWhPackage
    {
        /// <summary>
        /// 运单
        /// </summary>
        public string MawbNumber { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string Dest { get; set; }

        /// <summary>
        /// 总运单片块数
        /// </summary>
        public int Count { get; set; }
        public string FlightNumber { get; set; }
        public List<DsWhPackageList> ModList { get; set; }
    }
    public class DsWhPackage
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
        /// b包裹编号
        /// </summary>
        public string PackageNumber { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalNum { get; set; }
        /// <summary>
        /// 总重量
        /// </summary>
        public decimal TotalWeight { get; set; }
        /// <summary>
        /// 服务类型
        /// </summary>
        public string ServiceType { get; set; }
        /// <summary>
        /// 最后扫描货物编号
        /// </summary>
        public string LastScanned { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 装包时间
        /// </summary>
        public DateTime EntryDatetime { get; set; }
        /// <summary>
        /// 客户编码
        /// </summary>
        public string CusCode { get; set; }
    }
}
