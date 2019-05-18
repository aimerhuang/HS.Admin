using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 联盟广告导出Excel
    /// </summary>
    ///<remarks>2013-10-16 周唐炬 创建</remarks>
    public class CBUnAdvertisementLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 网站
        /// </summary>
        [Description("网站")]
        public string SiteName { get; set; }
        /// <summary>
        /// 网站Url
        /// </summary>
        [Description("网站Url")]
        public string SiteUrl { get; set; }
        /// <summary>
        /// 联盟广告系统编号
        /// </summary>
        [Description("联盟广告系统编号")]
        public int AdvertisementSysNo { get; set; }
        /// <summary>
        /// 广告类型,CPC(10)，CPA(20),CPS(30)
        /// </summary>
        [Description("广告类型,CPC(10)，CPA(20),CPS(30)")]
        public int AdvertisementType { get; set; }
        /// <summary>
        /// 访问IP
        /// </summary>
        [Description("访问IP")]
        public string AccessIp { get; set; }
        /// <summary>
        /// 访问时间
        /// </summary>
        [Description("访问时间")]
        public DateTime AccessTime { get; set; }
        /// <summary>
        /// 来源Url
        /// </summary>
        [Description("来源Url")]
        public string UrlReferrer { get; set; }
        /// <summary>
        /// 访问Url
        /// </summary>
        [Description("访问Url")]
        public string AccessUrl { get; set; }
        /// <summary>
        /// Cookie标识码
        /// </summary>
        [Description("Cookie标识码")]
        public string CookieIdentity { get; set; }
        /// <summary>
        /// 有效数据
        /// </summary>
        [Description("有效数据")]
        public string ValidData { get; set; }
        /// <summary>
        /// 有效金额
        /// </summary>
        [Description("有效金额")]
        public decimal ValidAmount { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        [Description("是否有效")]
        public int IsValid { get; set; }
        /// <summary>
        /// 更新IP
        /// </summary>
        [Description("更新IP")]
        public string UpdateIp { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Description("更新时间")]
        public DateTime UpdateTime { get; set; }
    }
}
