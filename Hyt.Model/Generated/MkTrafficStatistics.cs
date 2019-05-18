
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2014-01-09 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class MkTrafficStatistics
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 游客标识
        /// </summary>
        [Description("游客标识")]
        public string VisitorsSign { get; set; }
        /// <summary>
        /// 页面地址
        /// </summary>
        [Description("页面地址")]
        public string UrlAddress { get; set; }
        /// <summary>
        /// 访问时间
        /// </summary>
        [Description("访问时间")]
        public DateTime VisitingTime { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        [Description("IP地址")]
        public string IpAddress { get; set; }
        /// <summary>
        /// IP归属地
        /// </summary>
        [Description("IP归属地")]
        public string IpLocation { get; set; }
        /// <summary>
        /// 来源地址
        /// </summary>
        [Description("来源地址")]
        public string UrlReferrer { get; set; }
        /// <summary>
        /// 用户代理内容
        /// </summary>
        [Description("用户代理内容")]
        public string UserAgent { get; set; }
        /// <summary>
        /// 用户操作系统
        /// </summary>
        [Description("用户操作系统")]
        public string UserOS { get; set; }
        /// <summary>
        /// 浏览器类型
        /// </summary>
        [Description("浏览器类型")]
        public string BrowserType { get; set; }
        /// <summary>
        /// 浏览器版本
        /// </summary>
        [Description("浏览器版本")]
        public string BrowerVersion { get; set; }
        /// <summary>
        /// 是否是移动平台:是(1),否(0)
        /// </summary>
        [Description("是否是移动平台:是(1),否(0)")]
        public int IsMobileDevice { get; set; }
        /// <summary>
        /// 客户屏幕分辨率
        /// </summary>
        [Description("客户屏幕分辨率")]
        public string Screen { get; set; }
        /// <summary>
        /// 页面加载时间
        /// </summary>
        [Description("页面加载时间")]
        public int LoadTime { get; set; }
        /// <summary>
        /// 客户系统编号
        /// </summary>
        [Description("客户系统编号")]
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Description("性别")]
        public string Gender { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        [Description("年龄")]
        public int Age { get; set; }
        /// <summary>
        /// 来源域
        /// </summary>
        [Description("来源域")]
        public string SourceDomain { get; set; }
        /// <summary>
        /// 搜索关键字
        /// </summary>
        [Description("搜索关键字")]
        public string SeoKeyword { get; set; }
    }
}

