using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 团购查询参数
    /// </summary>
    /// <remarks>2013-08-20 朱家宏 创建</remarks>
    public struct ParaGroupShoppingFilter
    {
        private int _pageSize;
        private DateTime? _endDate;

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
        /// 状态
        /// </summary>
        public IList<int> Statuses { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 起始时间(起)
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 起始时间(止)
        /// </summary>
        public DateTime? EndDate
        {
            get
            {
                //查询日期上限+1
                return _endDate == null ? (DateTime?)null : _endDate.Value.AddDays(1);
            }
            set { _endDate = value; }
        }
    }
}
