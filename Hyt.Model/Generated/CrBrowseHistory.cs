
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2013-08-27 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class CrBrowseHistory
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 分类编号
        /// </summary>
        [Description("分类编号")]
        public int CategorySysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        [Description("商品编号")]
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 搜索关键字
        /// </summary>
        [Description("搜索关键字")]
        public string KeyWord { get; set; }
        /// <summary>
        /// 浏览次数
        /// </summary>
        [Description("浏览次数")]
        public int BrowseNum { get; set; }
        /// <summary>
        /// 浏览方式：PC网站（10）、信营全球购B2B2C3G网站（15）
        /// </summary>
        [Description("浏览方式：PC网站（10）、信营全球购B2B2C3G网站（15）")]
        public int BrowseType { get; set; }
        /// <summary>
        /// 是否翻页
        /// </summary>
        [Description("是否翻页")]
        public int IsPageDown { get; set; }
    }
}

