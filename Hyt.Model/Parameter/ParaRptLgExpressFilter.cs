
using System;
using System.Collections.Generic;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 报表-运营综述查询参数
    /// </summary>
    /// <remarks>2013-10-29 余勇 创建</remarks>
    public struct ParaRptLgExpressFilter
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
        /// 开始日期
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate
        {
            get
            {
                //结束+1
                return _endDate == null ? (DateTime?)null : _endDate.Value.AddDays(1);
            }
            set { _endDate = value; }
        }
        /// <summary>
        /// 字段类型 1为成功、2为失败、空为总单量
        /// </summary>
        public int? DataType { get; set; }

        /// <summary>
        /// 统计月份
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 统计年份
        /// </summary>
        public string Year { get; set; }
    }
}
