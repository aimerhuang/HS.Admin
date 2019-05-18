using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 联盟网站管理条件过滤器
    /// </summary>
    /// <remarks>2013-10-17 周唐炬 创建</remarks>
    public class ParaUnUnionSiteFilter
    {
        /// <summary>
        /// 网站
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// 状态:启用(1),禁用(0)
        /// </summary>
        public int? Status { get; set; }
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
