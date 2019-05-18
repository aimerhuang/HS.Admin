using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 新闻分类部分类
    /// </summary>
    /// <remarks>
    /// 2016-06-06 罗远康 创建
    /// </remarks>
    public partial class FeNewsCategory
    {
        /// <summary>
        /// 新闻父级分类
        /// </summary>
        public FeNewsCategory ParentCategory { get; set; }
    }
}
