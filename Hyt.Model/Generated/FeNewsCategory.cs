using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 新闻分类
    /// </summary>
    /// <remarks>
    /// 2016-06-07 罗远康 创建
    /// </remarks>
    [Serializable]
    public partial class FeNewsCategory
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 父级编号
        /// </summary>
        [Description("父级编号")]
        public int ParentSysNo { get; set; }
        /// <summary>
        /// 系统编号路由(,上级系统编号,...,系统编号,)
        /// </summary>
        [Description("系统编号路由")]
        public string SysNos { get; set; }
        /// <summary>
        /// 分销商编号
        /// </summary>
        [Description("分销商编号")]
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        [Description("分类名称")]
        public string CategoryName { get; set; }
        /// <summary>
        /// 分类图片地址
        /// </summary>
        [Description("分类图片地址")]
        public string CategoryImage { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        [Description("显示顺序")]
        public int DisplayOrder { get; set; }
        /// <summary>
        /// SEO标题
        /// </summary>
        [Description("SEO标题")]
        public string SeoTitle { get; set; }
        /// <summary>
        /// SEO关键字
        /// </summary>
        [Description("SEO关键字")]
        public string SeoKeyword { get; set; }
        /// <summary>
        /// SEO描述
        /// </summary>
        [Description("SEO描述")]
        public string SeoDescription { get; set; }
        /// <summary>
        /// 状态：有效（1）、无效（0）
        /// </summary>
        [Description("状态：有效（1）、无效（0）")]
        public int Status { get; set; }
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

