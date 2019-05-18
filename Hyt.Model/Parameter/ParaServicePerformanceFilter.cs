using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 客服绩效报表查询参数
    /// </summary>
    /// <remarks>2013-12-11 黄志勇 创建</remarks>
    public class ParaServicePerformanceFilter
    {
        private int _pageSize;

        /// <summary>
        /// 当前页号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get
            {
                if (_pageSize == 0)
                {
                    _pageSize = 10;
                }
                return _pageSize;
            }
            set { _pageSize = value; }
        }

        /// <summary>
        /// 统计日期
        /// </summary>
        public string Reptdt { get; set; }

        /// <summary>
        /// 客服系统编号
        /// </summary>
        public int? ServiceNo { get; set; }
   
    }
}
