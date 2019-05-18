using System;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 退款订单查询参数
    /// </summary>
    /// <remarks>2013-09-10 余勇 创建</remarks>
    public struct ParaDsReturnFilter
    {
        private int _pageSize;
        private DateTime? _endDate;

        /// <summary>
        /// 当前页号
        /// </summary>
        public int PageIndex { get; set; }

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
        /// 商城商品编号
        /// </summary>
        public string MallProductId { get; set; }

        /// <summary>
        /// 商城订单编号
        /// </summary>
        public string MallOrderId { get; set; }

        /// <summary>
        /// 商城商品名称
        /// </summary>
        public string MallProductName { get; set; }

        /// <summary>
        /// 买家昵称
        /// </summary>
        public string BuyerNick { get; set; }

        /// <summary>
        /// 升舱时间(起)
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 升舱时间(止)
        /// </summary>
        public DateTime? EndDate
        {
            get
            {
                //查询日期上限+1
                return _endDate == null ? (DateTime?) null : _endDate.Value.AddDays(1);
            }
            set { _endDate = value; }
        }

        /// <summary>
        /// 分销商商城编号
        /// </summary>
        public int DealerMallSysNo { get; set; }

        /// <summary>
        /// 退换货类型
        /// </summary>
        /// <remarks>2017-8-29 罗熙 创建</remarks>
        public int RmaType { get; set; }
    }

}
