using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 商品描述模板扩展属性类
    /// </summary>
    /// <remarks>
    /// 2013-07-22 杨晗 创建
    /// </remarks>
    public class CBPdTemplate : PdTemplate
    {
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedByName { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        public string LastUpdateByName { get; set; }
    }
}
