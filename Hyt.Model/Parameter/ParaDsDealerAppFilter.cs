using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 分销商App查询
    /// </summary>
    /// <remarks>2014-05-05 余勇 创建</remarks>
    public class ParaDsDealerAppFilter
    {
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
