using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Order
{
    /// <summary>
    /// 订单获取接口查询字段实体
    /// </summary>
    /// 2018-2-2 吴琨 创建
    public class GetOrderApi
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }
       
        /// <summary>
        /// 订单开始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 订单结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int? status { get; set; }


        /// <summary>
        /// 订单支付状态
        /// </summary>
        public int? paymentStatus { get; set; }

        /// <summary>
        /// 供应商Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 仓库编号集合
        /// </summary>
        public List<int> whList { get; set; }


    }


    public class GetOrderApiRtn
    {

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string AddDate { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ConsigneeName { get; set; }

        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string ConsigneeMobile { get; set; }

        /// <summary>
        /// 收货人身份证
        /// </summary>
        public string ConsigneeIDCardNo { get; set; }

        /// <summary>
        /// 收货人省市区
        /// </summary>
        public string ConsigneeRegion { get; set; }

        /// <summary>
        /// 收货人详细地址
        /// </summary>
        public string ConsigneeAddress { get; set; }


        /// <summary>
        /// 物流配送方名称
        /// </summary>
        public string DeliveryTypeName { get; set; }

        /// <summary>
        /// 物流快递单号
        /// </summary>
        public string ExpressNo { get; set; }

        /// <summary>
        /// 物流运费金额
        /// </summary>
        public decimal freight { get; set; }

        /// <summary>
        /// 销售单总金额
        /// </summary>
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public string PayStatus { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatus { get; set; }

        /// <summary>
        /// 订单商品明细
        /// </summary>
        public List<GetOrderApiProduct> Product { get; set; }
    }

    /// <summary>
    /// 订单查询接口商品信息实体
    /// </summary>
    public class GetOrderApiProduct
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public string ProductCode{get;set;}

        /// <summary>
        /// 商品条码
        /// </summary>
        public string ProductBarcode{get;set;}
       
        /// <summary>
        /// 规格
        /// </summary>
        public string Specifications { get; set; }

        /// <summary>
        /// 订购数量
        /// </summary>
        public int Count { get; set; }
      

        /// <summary>
        /// 原单价
        /// </summary>
        public decimal OriginalUnitPrice { get; set; }

        /// <summary>
        /// 销售价
        /// </summary>
        public decimal SalesAmount { get; set; }
    }
}
