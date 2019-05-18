using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 配货(拣货，出库)单打印实体
    /// </summary>
    /// <remarks>
    /// 2013-07-16 郑荣华 创建
    /// </remarks>
    public class CBPrtPicking : WhStockOut
    {
        /// <summary>
        /// 会员名称
        /// </summary>
        public string Name { set; get; }

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
        /// 店铺名称，本公司用 商城 
        /// </summary>
        public string ShopName
        {
            get { return "商城"; }
            set { value = ""; }
        }

        /// <summary>
        /// 金额总计
        /// </summary>
        public decimal MoneyCount { get; set; }

        /// <summary>
        /// 数量总计
        /// </summary>
        public int QuantityCount { get; set; }

        /// <summary>
        /// 明细列表
        /// </summary>
        public IList<PrtSubPicking> List;
    }

    /// <summary>
    /// 配货(拣货，出库)单打印明细实体
    /// </summary>
    /// <remarks>
    /// 2013-07-17 郑荣华 创建
    /// </remarks>
    public class PrtSubPicking : WhStockOutItem
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
