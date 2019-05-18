using System;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 升舱订单查询参数
    /// </summary>
    /// <remarks>2013-09-03 朱家宏 创建</remarks>
    public struct ParaDsEasFilter
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
                return _endDate == null ? (DateTime?) null : _endDate.Value.AddDays(1);
            }
            set { _endDate = value; }
        }

        /// <summary>
        /// 状态:启用(1),禁用(0)
        /// </summary>
        public new int? Status { get; set; }

        /// <summary>
        /// 卖家昵称
        /// </summary>
        public string SellerNick { get; set; }

        /// <summary>
        /// 分销商城类型系统编号
        /// </summary>
        public int? MallTypeSysNo { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 店铺账号
        /// </summary>
        public string ShopAccount { get; set; }

        /// <summary>
        /// EAS编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 是否使用旗帜
        /// </summary>
        public bool IsUseFlag { get; set; }
    }

}
