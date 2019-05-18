using System;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 升舱订单查询参数
    /// </summary>
    /// <remarks>2013-09-03 朱家宏 创建</remarks>
    public struct ParaDsOrderFilter
    {
        private int _pageSize;
        private DateTime? _endDate;
        private string _mallOrderId;
        private string _mallProductId;

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
        /// 状态
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        /// 商城商品编号
        /// </summary>
        public string MallProductId
        {
            get { return string.IsNullOrEmpty(_mallProductId) ? string.Empty : _mallProductId.Trim(); }
            set { _mallProductId = value; }
        }

        /// <summary>
        /// 商城订单编号
        /// </summary>
        public string MallOrderId
        {
            get
            {
                return string.IsNullOrEmpty(_mallOrderId)
                           ? string.Empty
                           : _mallOrderId.Trim();
            }
            set { _mallOrderId = value; }
        }

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
        /// 汇源提订单状态
        /// </summary>
        public int? HytOrderStatus { get; set; }

        /// <summary>
        /// 分销商商城编号
        /// </summary>
        public int? DealerMallSysNo { get; set; }

        /// <summary>
        ///商城订单号
        /// </summary>
        public int? OrderSysNo { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ReceiveName { get; set; }

        /// <summary>
        /// 收货人手机
        /// </summary>
        public string MobilePhoneNumber { get; set; }
    }

}
