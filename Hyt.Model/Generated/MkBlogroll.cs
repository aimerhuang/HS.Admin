
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 友情链接管理
    /// </summary>
    /// <remarks>
    /// 2013-12-10 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class MkBlogroll
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 网站名称
        /// </summary>
        [Description("网站名称")]
        public string WebSiteName { get; set; }
        /// <summary>
        /// 网站地址
        /// </summary>
        [Description("网站地址")]
        public string WebSiteUrl { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Description("电子邮箱")]
        public string EmailAddress { get; set; }
        /// <summary>
        /// 网站描述
        /// </summary>
        [Description("网站描述")]
        public string SiteDescription { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        public int DisplayOrder { get; set; }
        /// <summary>
        /// 状态:待审(10),已审(20),作废(-10)
        /// </summary>
        [Description("状态:待审(10),已审(20),作废(-10)")]
        public int Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        [Description("审核人")]
        public int AuditorSysNo { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        [Description("审核时间")]
        public DateTime AuditDate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Description("创建人")]
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Description("最后更新时间")]
        public DateTime LastUpdateDate { get; set; }
    }
}

