using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    ///筛选 
    /// </summary>
    /// <remarks>2014-8-22 朱成果 创建</remarks>
  public  class ParaReconciliationFilter
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
        /// 财务流水号
        /// </summary>
        public string FnNo { get; set; }

       /// <summary>
       /// 订单号
       /// </summary>

        public string TraderNo { get; set; }

        /// <summary>
        /// 开始时间(起)
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 结束时间(止)
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
          /// <summary>
          /// 来源
          /// </summary>
        public int? Source { get; set; }

       /// <summary>
       /// 状态
       /// </summary>

        public int? Status { get; set; }

        
    }
}
