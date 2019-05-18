using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 商品浏览历史查询条件实体
    /// </summary>
    /// <remarks>
    /// 2013-09-10 郑荣华 创建
    /// </remarks>
    public class ParaCrBrowseHistoryFilter
    {
        /// <summary>
        /// 顾客系统编号
        /// </summary>
        public int? CustomerSysNo { get; set; }

        /// <summary>
        /// 分类编号
        /// </summary>
        public int? CategorySysNo { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public int? ProductSysNo { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string KeyWord { get; set; }
 
        /// <summary>
        /// 浏览方式：PC网站（10）、信营全球购B2B2C3G网站（15）
        /// </summary>
        public int? BrowseType { get; set; }

        /// <summary>
        /// 是否翻页
        /// </summary>     
        public int? IsPageDown { get; set; }
    }
}
