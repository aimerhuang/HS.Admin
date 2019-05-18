
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 广告日志
    /// </summary>
    /// <remarks>
    /// 2013-10-14 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class UnAdvertisementLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
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

