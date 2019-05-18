using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 电商中心绩效报表查询参数
    /// </summary>
    /// <remarks>2013-12-10 黄志勇 创建</remarks>
    public class ParaEBusinessCenterPerformanceFilter
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
        /// 开始时间
        /// </summary>
        public DateTime? DateStart { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? DateEnd { get; set; }

        /// <summary>
        /// 选择月份
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 分销商商城编号
        /// </summary>
        public int? DealerMallSysNo { get; set; }
    }
}
