using System;
using System.Collections.Generic;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 出库单筛选字段
    /// </summary>
    /// <remarks>2013-07-06 朱家宏 创建</remarks>
    public class ParaOutStockOrderFilter
    {
        private DateTime? _orderEndDate;
        private DateTime? _pickUpEndDate;
        private string _keyword;

        /// <summary>
        /// 订单号
        /// </summary>
        public int? OrderSysNo { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int? OutStockOrderStatus { get; set; }

        /// <summary>
        /// 会员会员号(登录帐号)
        /// </summary>
        public string CustomerAccount { get; set; }

        /// <summary>
        /// 配送方式
        /// </summary>
        public IList<int> DeliveryTypeSysNoList { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 收货电话
        /// </summary>
        public string ReceiverTel { get; set; }

        /// <summary>
        /// 出库单号
        /// </summary>
        public int? WhStockOutSysNo { get; set; }

        /// <summary>
        /// 下单日期(起)
        /// </summary>
        public DateTime? OrderBeginDate { get; set; }

        /// <summary>
        /// 下单日期(止)
        /// </summary>
        public DateTime? OrderEndDate
        {
            get
            {
                //上限+1
                return _orderEndDate == null ? (DateTime?) null : _orderEndDate.Value.AddDays(1);
            }
            set { _orderEndDate = value; }
        }

        /// <summary>
        /// 延时自提时间(起)
        /// </summary>
        public DateTime? PickUpBeginDate { get; set; }

        /// <summary>
        /// 延时自提时间(止)
        /// </summary>
        public DateTime? PickUpEndDate
        {
            get
            {
                //上限+1
                return _pickUpEndDate == null ? (DateTime?) null : _pickUpEndDate.Value.AddDays(1);
            }
            set { _pickUpEndDate = value; }
        }

        /// <summary>
        /// 搜索订单号,收货手机，收货人
        /// </summary>
        public string Keyword
        {
            get { return string.IsNullOrEmpty(_keyword) ? string.Empty : _keyword.Trim(); }
            set { _keyword = value; }
        }

        /// <summary>
        /// 门店查询（SysNo）
        /// </summary>
        public IList<int> StoreSysNoList { get; set; }

        /// <summary>
        /// 出库单状态值
        /// </summary>
        public int? StockOutStatus { get; set; }

        /// <summary>
        /// 需排除的出库单状态
        /// </summary>
        public IList<int> StockOutStatusListExcepted { get; set; }

        /// <summary>
        /// 用户可查看的仓库列表(门店)
        /// </summary>
        public IList<WhWarehouse> Warehouses { get; set; }

        /// <summary>
        /// 查询类型(快速检索中下拉列表值)
        /// </summary>
        public int? QueryType { get; set; }

        /// <summary>
        /// 当前页号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 签收时间
        /// </summary>
        public DateTime? SignTime { get; set; }

    }
}
