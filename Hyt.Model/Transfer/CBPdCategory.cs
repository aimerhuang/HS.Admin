using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 商品分类扩展方法
    /// </summary>
    [Serializable]
    public class CBPdCategory : PdCategory
    {
        /// <summary>
        /// 分类颜色
        /// </summary>
        public string Color { get; set; }
    }
}
