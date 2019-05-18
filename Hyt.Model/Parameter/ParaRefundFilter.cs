using System;
using System.Collections.Generic;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 退款
    /// </summary>
    public class ParaRefundFilter
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
        /// 退款编号
        /// </summary>
        public int? SysNo { get; set; }
        /// <summary>
        /// 退款开始日期(起)
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 退款结束日期(止)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public int? OrderSysNo { get; set; }

        /// <summary>
        /// 退款状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 申请单处理部门:客服中心(10),门店(20)
        /// </summary>
        public int? HandleDepartments { get; set; }

        /// <summary>
        /// 会员编号
        /// </summary>
        public int? CustomerSysNo { get; set; }

        /// <summary>
        /// 申请单来源:会员(10),客服(20),门店(30)
        /// </summary>
        public int? Source { get; set; }

    }
}
