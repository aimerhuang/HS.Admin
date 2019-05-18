using System;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 财务管理单据查询参数
    /// </summary>
    /// <remarks>2013-07-19 朱家宏 创建</remarks>
    /// <remarks>2013-07-24 黄志勇 修改</remarks>
    public struct ParaVoucherFilter
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
        /// 收入类型
        /// </summary>
        public int? IncomeType { get; set; }

        /// <summary>
        /// 单据来源
        /// </summary>
        public int? Source { get; set; }

        /// <summary>
        /// 单据状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 开始日期(起)
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 结束时间(止)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 单据来源编号
        /// </summary>
        public int? SourceSysNo { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        /// <remarks>2013-12-23 朱家宏 添加</remarks>
        public int? PaymentType { get; set; }
    }
}
