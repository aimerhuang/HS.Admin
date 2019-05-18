using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.LiJiaModel
{
    /// <summary>
    /// 商品分类利嘉模板新增
    /// </summary>
    /// <remarks>
    /// 2017-05-18 罗勤尧 生成
    /// </remarks>
    public class ProductCategoryLiJia
    {
        /// <summary>
        /// 上级分类id,根级用0
        /// </summary>
        [Description("上级分类id,根级用0")]
        public int ParentCategoryId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        [Description("分类名称")]
        public string CategoryName { get; set; }
    }
}
