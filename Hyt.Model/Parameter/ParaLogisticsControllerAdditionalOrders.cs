using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 用于补单操作参数类
    /// </summary>
    /// <remarks>
    /// 2013-07-16 沈强 创建
    /// </remarks> 
    public class ParaLogisticsControllerAdditionalOrders:BaseEntity
    {
        /// <summary>
        /// 仓库系统编号
        /// </summary>
        public int WarehouseSysNo { get; set; }
        /// <summary>
        /// 配送员系统编号
        /// </summary>
        public int DeliverymanSysNo { get; set; }
        /// <summary>
        /// 会员系统编号
        /// </summary>
        public int UserSysNo { get; set; }
        /// <summary>
        /// 收货地址信息
        /// </summary>
        public ReceiveAddress ReceiveAddress { get; set; }
        /// <summary>
        /// 支付方式系统编号
        /// </summary>
        public int PaymentTypeSysNo { get; set; }
        /// <summary>
        /// 订单信息集合
        /// </summary>
        public List<OrderInformation> OrderInformations { get; set; }
        /// <summary>
        /// 会员等级系统编号
        /// </summary>
        public int LevelSysNo { get; set; }
    }

    /// <summary>
    /// 补单收货地址信息
    /// </summary>
    /// <remarks>
    /// 2013-07-16 沈强 创建
    /// </remarks> 
    public class ReceiveAddress : BaseEntity
    {
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string MobilePhoneNumber { get; set; }
        /// <summary>
        /// 收货人座机号
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 收货人所在地区系统编号
        /// </summary>
        public int AreaSysNo { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode { get; set; }
    }

    /// <summary>
    /// 商品订购信息
    /// </summary>
    /// <remarks>
    /// 2013-07-16 沈强 创建
    /// </remarks> 
    public class OrderInformation : BaseEntity
    {
        /// <summary>
        /// 商品系统编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品订购数量
        /// </summary>
        public int ProductOrderNumber { get; set; }
    }
}
