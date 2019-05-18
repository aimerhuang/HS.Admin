using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.SearchAttribute;

namespace Hyt.Model
{
    /// <summary>
    /// 搜索属性以及属性选项
    /// 用于前台
    /// </summary>
    /// <remarks>2013-08-22 黄波 创建</remarks>
    [Serializable]
    public  class SearchAttributeAndOptions
    {
        /// <summary>
        /// 属性系统编号
        /// </summary>
        public int AttributeSysNo { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string AttributeName { get; set; }
        /// <summary>
        /// 属性选项
        /// </summary>
        public IList<SearchAttributeOption> Options { get; set; }
    }
}
