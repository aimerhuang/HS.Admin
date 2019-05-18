using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 文章类型扩展类
    /// </summary>
    /// <remarks>
    /// 2013-08-12 杨晗 创建
    /// </remarks>
    public class CBFeArticleCategory : FeArticleCategory
    {
        /// <summary>
        /// 一个文章类别对应多个文章
        /// </summary>
        public IList<FeArticle> FeArticle { get; set; }
    }
}
