
using System;
using System.Collections.Generic;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 报表-运营综述查询参数
    /// </summary>
    /// <remarks>2013-10-29 余勇 创建</remarks>
    public struct ParaRptBusinessSummaryFilter
    {
        private DateTime? _endDate;

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
                return _endDate == null ? (DateTime?) null : _endDate.Value.AddDays(1);
            }
            set { _endDate = value; }
        }
        /// <summary>
        /// 字段类型
        /// </summary>
        public int? DataType { get; set; }

        /// <summary>
        /// 排序（升序为1，降序为0）
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 开始月(月报)
        /// </summary>
        /// <remarks>2014-04-23 朱家宏 创建</remarks>
        public string StartMonth { get; set; }

        /// <summary>
        /// 结束月(月报)
        /// </summary>
        /// <remarks>2014-04-23 朱家宏 创建</remarks>
        public string EndMonth { get; set; }
    }
}
