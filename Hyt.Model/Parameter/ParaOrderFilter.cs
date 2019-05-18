using System;
using System.Collections.Generic;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 订单筛选字段
    /// </summary>
    /// <remarks>2013-06-24 朱家宏 创建</remarks>
    public class ParaOrderFilter
    {
        private DateTime? _endDate;
        private DateTime? _payendDate;
        private string _keyword;
        private string _customerMobile;
        private string _mallOrderId;
        private string _mallShopName;

        /// <summary>
        /// 订单号
        /// </summary>
        public int? OrderSysNo { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int? OrderStatus { get; set; }
        /// <summary>
        /// 订单状态集合 2016-4-6 杨云奕 添加
        /// </summary>
        public string OrderStatusList { get; set; }
        /// <summary>
        /// 是否过滤作废订单
        /// </summary>
        public int? NonInvalidStatus { get; set; }

        /// <summary>
        /// 会员姓名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 会员会员号(登录帐号)
        /// </summary>
        public string CustomerAccount { get; set; }

        /// <summary>
        /// 会员手机号
        /// </summary>
        public string CustomerMobile
        {
            get
            {
                return string.IsNullOrEmpty(_customerMobile)
                           ? string.Empty
                           : _customerMobile.Replace(" ", "").Replace("　", "");
            }
            set { _customerMobile = value; }
        }

        /// <summary>
        /// 任务池中的订单执行人(not null 为“我的订单”)
        /// </summary>
        public int? ExecutorSysNo { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public int? OrderSource { get; set; }

        /// <summary>
        /// 订单来源编号
        /// </summary>
        public IList<int> OrderSourceSysNoList { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        public int? PayTypeSysNo { get; set; }

        /// <summary>
        /// 配送类型
        /// </summary>
        public int? DeliveryTypeSysNo { get; set; }

        /// <summary>
        /// 需要排除的配送类型 (不为null时进行"不等于"查询)
        /// </summary>
        public int? ExceptedDeliveryTypeSysNo { get; set; }

        /// <summary>
        /// 订单创建人
        /// </summary>
        public string OrderCreator { get; set; }

        /// <summary>
        /// 订单审核人
        /// </summary>
        public string Auditor { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string ReceiveName { get; set; }

        /// <summary>
        /// 收货电话
        /// </summary>
        public string ReceiveTel { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 出库单号
        /// </summary>
        public int? WhStockOutSysNo { get; set; }

        /// <summary>
        /// 订单总额下限
        /// </summary>
        public decimal? MinOrderAmount { get; set; }

        /// <summary>
        /// 订单总额上限
        /// </summary>
        public decimal? MaxOrderAmount { get; set; }

        /// <summary>
        /// 订单创建日(起)
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 订单创建日(止)
        /// </summary>
        public DateTime? EndDate
        {
            get
            {
                //查询日期上限+1
                return _endDate == null ? (DateTime?) null : _endDate.Value;
            }
            set { _endDate = value; }
        }

        /// <summary>
        /// 查询关键字
        /// </summary>
        public string Keyword
        {
            get { return string.IsNullOrEmpty(_keyword) ? string.Empty : _keyword.Trim(); }
            set { _keyword = value; }
        }

        /// <summary>
        /// 订单出库仓库(门店)查询 (为0表示为缺货订单)
        /// </summary>
        //public IList<int> DefaultWarehouseSysNoList { get; set; }

        /// <summary>
        /// 当前页号
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 当前页号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 订单出库仓库(门店)查询 (为0表示为缺货订单)
        /// </summary>
        public IList<int> StoreSysNoList { get; set; }

        /// <summary>
        /// 查询类型
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 用户可查看的仓库列表(门店)
        /// </summary>
        public IList<WhWarehouse> Warehouses { get; set; }

        /// <summary>
        /// 会员编号
        /// </summary>
        public int? CustomerSysNo { get; set; }

        /// <summary>
        /// 结算单状态（在创建退换货单查询时赋值使用）
        /// </summary>
        public int? SettlementStatus { get; set; }

        /// <summary>
        /// 处理部门，门店为20
        /// </summary>
        public int? HandleDepartment { get; set; }

        /// <summary>
        /// 任务对象类型 (订单任务池)
        /// </summary>
        public IList<int> TaskTypes
        {
            get
            {
                //在"我的订单"查询时需要该属性支持
                return ExecutorSysNo != null
                           ? new List<int>
                               {
                                   (int) SystemStatus.任务对象类型.客服订单提交出库,
                                   (int) SystemStatus.任务对象类型.客服订单审核
                               }
                           : null;
            }
        }

        /// <summary>
        /// 支付类型(BasicStatus.支付方式类型 预付或到付)
        /// </summary>
        public int? PaymentType { get; set; }

        /// <summary>
        /// 运单号查询
        /// </summary>
        public string ExpressNo { get; set; }

        /// <summary>
        /// 商城订单号
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
        /// 第三方商城店铺名称
        /// </summary>
        public string MallShopName
        {
            get
            {
                return string.IsNullOrEmpty(_mallShopName)
                           ? string.Empty
                           : _mallShopName.Trim();
            }
            set { _mallShopName = value; }
        }

        /// <summary>
        /// 仓库编号
        /// </summary>
        /// <remarks>2014-07-31 余勇 创建</remarks>
        public int? WarehouseSysNo { get; set; }

        /// <summary>
        /// 当前分销商系统编号
        /// </summary>
        /// <remarks>2015-12-16 王耀发 创建</remarks>
        public int DealerSysNo { get; set; }

        /// <summary>
        /// 分销商编号集合
        /// </summary>
        public List<int> DealerSysNoList { get; set; }

        /// <summary>
        /// 是否绑定经销商
        /// </summary>
        public bool IsBindDealer { get; set; }
        /// <summary>
        /// 是否绑定所有经销商
        /// </summary>
        public bool IsBindAllDealer { get; set; }
        /// <summary>
        /// 经销商创建人
        /// </summary>
        public int DealerCreatedBy { get; set; }
        /// <summary>
        /// 搜索条件选中的分销商
        /// </summary>
        public int SelectedDealerSysNo { get; set; }
        /// <summary>
        /// 搜索条件选中的代理商
        /// </summary>
        public int SelectedAgentSysNo { get; set; }
        /// <summary>
        /// 订单支付日(起)
        /// </summary>
        public DateTime? PayBeginDate { get; set; }

        /// <summary>
        /// 订单支付日(止)
        /// </summary>
        public DateTime? PayEndDate
        {
            get
            {
                //查询日期上限+1
                return _payendDate == null ? (DateTime?)null : _payendDate.Value;
            }
            set { _payendDate = value; }
        }

        /// <summary>
        /// 订单支付状态
        /// </summary>
        public int? PayStatus { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 供应链编号
        /// </summary>
        public int Supply { get; set; }

        /// <summary>
        /// 是否绑定所有仓库
        /// </summary>
        public bool HasAllWarehouse { get; set; }
        /// <summary>
        /// 商品ERP编号 2017-10-11 罗勤瑶 添加
        /// </summary>
        public string ErpCode { get; set; }
    }

}
