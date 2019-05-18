using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 新闻扩展属性类
    /// </summary>
    /// <remarks>
    /// 2014-01-15 苟治国 创建
    /// </remarks>
    public class CBFeNews:FeNews
    {
        /// <summary>
        /// 新闻创建人
        /// </summary>
        /// <remarks>2014-01-15 苟治国 创建</remarks>
        public string CreatedByName { get; set; }
    }
}
