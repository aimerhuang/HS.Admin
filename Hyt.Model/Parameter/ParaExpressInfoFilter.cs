using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
  /// <summary>
  /// 快递100明细报表传参数
  /// </summary>
  /// <remarks>2014-0-05-20 朱成果 创建</remarks> 
    public   class ParaExpressInfoFilter
    {
        /// <summary>
        /// 页号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool? IsSuccess { get; set; }

    }
}
