using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 商品分类部分类
    /// </summary>
    /// <remarks>
    /// 2013-07-05 邵斌 创建
    /// </remarks>
    public partial class PdCategory
    {
        /// <summary>
        /// 商品父级分类
        /// </summary>
        public PdCategory ParentCategory { get; set; }

        /// <summary>
        /// 是否是主分类
        /// </summary>
        public bool IsMaster { get; set; }
    }
}
