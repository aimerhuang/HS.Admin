using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 分销配货(拣货，出库)单打印实体
    /// </summary>
    /// <remarks>
    /// 2013-07-16 郑荣华 创建
    /// </remarks>
    public class PrtDsPicking : WhStockOut
    {
        /// <summary>
        /// 会员名称 
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 会员系统编号
        /// </summary>
        public int CustomerSysNo { set; get; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ReceiveName { get; set; }

        /// <summary>
        /// 收货人固定电话
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 收货人移动电话
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 收货地址区域编号
        /// </summary>
        public int AreaSysNo { get; set; }

        /// <summary>
        /// 收货地址街道信息
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string PaymentName { get; set; }

        /// <summary>
        /// 支付类型 预付（10）、到付（20）
        /// </summary>
        public int PaymentType { get; set; }

        /// <summary>
        /// 配送方式名称
        /// </summary>
        public string DeliveryTypeName { get; set; }

        /// <summary>
        /// 配送备注
        /// </summary>
        public string SoDeliveryRemarks { get; set; }

        /// <summary>
        /// 销售备注
        /// </summary>
        public string SoRemarks { get; set; }
        /// <summary>
        /// 运费（销售单）
        /// </summary>
        public decimal FreightAmount { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime SoCreatedDate { get; set; }

        /// <summary>
        /// 发货仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 发货后台仓库名称
        /// </summary>
        public string BackWarehouseName { get; set; }

        /// <summary>
        /// 订单事务编号
        /// </summary>
        public string OrderTransactionSysNo { get; set; }
        /// <summary>
        /// 商城订单号
        /// </summary>        
        public string MallOrderId { get; set; }

        /// <summary>
        /// 店铺账号
        /// </summary>   
        public string ShopAccount { get; set; }

        /// <summary>
        /// 店铺名称，本公司用 商城 
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 是否自营
        /// </summary>
        /// <remarks>2014-07-21 余勇 创建</remarks> 
        public int IsSelfSupport { get; set; }

        /// <summary>
        /// 商品明细列表
        /// </summary>
        public IList<PrtDsSubPicking> List;

        /// <summary>
        /// 分销订单明细列表（商城订单1对多分销订单）
        /// </summary>
        public IList<CBDsOrder> ListDs;

        /// <summary>
        /// 总数量
        /// </summary>
        public int QuantityCount { get; set; }

        /// <summary>
        /// 总重量
        /// </summary>
        public decimal WeightCount { get; set; }

        /// <summary>
        /// 金额总计
        /// </summary>
        /// <remarks>2014-07-21 余勇 创建</remarks> 
        public decimal MoneyCount { get; set; }
    }

    /// <summary>
    /// 分销配货(拣货，出库)单打印明细实体
    /// </summary>
    /// <remarks>
    /// 2013-07-17 郑荣华 创建
    /// </remarks>
    public class PrtDsSubPicking : WhStockOutItem
    {
        /// <summary>
        /// 商品ID（编号、ERP）
        /// </summary>
        public string ErpCode { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string WarehousePositionName { get; set; }
    }
}
