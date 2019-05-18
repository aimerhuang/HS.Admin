using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.SearchAttribute
{
    /// <summary>
    /// 属性选项
    /// </summary>
    /// <remarks>2013-08-22 黄波 创建</remarks>
    [Serializable]
    public class SearchAttributeOption
    {
        /// <summary>
        /// 属性选项系统编号
        /// </summary>
        public int AttributeOptionSysNo { get; set; }
        /// <summary>
        /// 属性选项文本值
        /// </summary>
        public string AttributeOptionText { get; set; }
    }
}
