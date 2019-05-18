using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 系统配置
    /// </summary>
    /// <remarks>2014-01-20 周唐炬 创建</remarks>
    public class ParaSyConfigFilter
    {
        /// <summary>
        /// 分类编号
        /// </summary>
        public int? CategoryId { get; set; }
        /// <summary>
        /// 当前页号
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
