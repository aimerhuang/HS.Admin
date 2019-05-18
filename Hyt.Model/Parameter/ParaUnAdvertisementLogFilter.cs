using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 广告查询
    /// </summary>
    /// <remarks>2014-1-15 沈强 创建</remarks>
    public class ParaUnAdvertisementLogFilter
    {
        /// <summary>
        /// 联盟网站系统编号
        /// </summary>
        public int? UnionSiteSysNo { get; set; }
        /// <summary>
        /// 联盟广告系统编号
        /// </summary>
        public int? AdvertisementSysNo { get; set; }
        /// <summary>
        /// 广告类型,CPC(10)，CPA(20),CPS(30)
        /// </summary>
        public int? AdvertisementType { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public int? IsValid { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时期
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 当前页号
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
