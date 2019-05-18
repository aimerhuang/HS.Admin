using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 电子运货单
    /// </summary>
    /// <remarks>2015-9-28 谭显锋 创建</remarks>
    public partial class LgElectronicWayBill
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 出库单编号
        /// </summary>
        public int WhStockOutSysNo { get; set; }
        /// <summary>
        /// 运货单单号
        /// </summary>
        public string WayBillNo { get; set; }      
        /// <summary>
        /// 大头笔
        /// </summary>
        public string MarkDestination { get; set; }
        /// <summary>
        /// 分拣站点名称
        /// </summary>
        public string SortingSiteName { get; set; }      
        /// <summary>
        /// 分拣站点编码
        /// </summary>
        public string SortingSiteCode { get; set; }      
        /// <summary>
        /// 分拣编码
        /// </summary>
        public string SortingCode { get; set; }      
        /// <summary>
        /// 集包编码
        /// </summary>
        public string PackageCode { get; set; }      
        /// <summary>
        /// 运单发放站点名称
        /// </summary>
        public string BillProvideSiteName { get; set; }
        /// <summary>
        /// 运单发放站点编码
        /// </summary>
        public string BillProvideSiteCode { get; set; }
       /// <summary>
        /// 运货单状态
       /// </summary>
        public int Status { get; set; }      
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateDate { get; set; }
        
    }
}
