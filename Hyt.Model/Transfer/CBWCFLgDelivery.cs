using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// wcfmodel配送单
    /// </summary>
    /// <remarks>2013-06-21 黄伟 创建</remarks>
    public class CBWCFLgDelivery
    {
        /*
                SysNo: "配送单号", 
                NoteNum:”单据数: 5”, /
                ShipAmount:”配送金额”, /
                ShipTime:"配送时间"		
                LogisticsDeliveryItems: [ //配送明细包括{出库单/取件单}
                {
 
         */
        /// <summary>
        /// 配送单号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 单据数--需计算
        /// </summary>
        public int NoteNum { get; set; }
        /// <summary>
        /// 配送金额
        /// </summary>
        public double ShipAmount { get; set; }
        /// <summary>
        /// 配送时间
        /// </summary>
        public DateTime ShipTime { get; set; }
        /// <summary>
        /// 配送单子表数据集合
        /// </summary>
        public List<LogisticsDeliveryItem> LogisticsDeliveryItems { get; set; }

        public int Status { get; set; }
    }

    /// <summary>
    /// wcfmodel配送单item
    /// </summary>
    /// <remarks>2013-06-21 黄伟 创建</remarks>
    public class LogisticsDeliveryItem
    {

        /// <summary>
        /// 配送单明细系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 单据类型{出库单/取件单}
        /// </summary>
        public int NoteType { get; set; }
        /// <summary>
        /// 单据系统编号
        /// </summary>
        public int NoteSysNo { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string AddrName { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string StreetAddress { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string MobilePhoneNumber { get; set; }
        /// <summary>
        /// 座机
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Latitude { get; set; }
        /// <summary>
        /// 配送状态/取件状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public int PaymentType { get; set; }
        ///// <summary>
        ///// 商品总价-需计算
        ///// </summary>
        //public double TotalPrice { get; set; }
        ///// <summary>
        ///// 优惠价格-需计算
        ///// </summary>
        //public double PreferentialPrice { get; set; }
        /// <summary>
        /// 实际金额-需计算
        /// </summary>
        public double PaymentAmount { get; set; }
        /// <summary>
        /// 商品
        /// </summary>
        public List<Item> Items { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public int OrderSysNo { get; set; }

        /// <summary>
        /// 出库单金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// app sign status,ref:enum 配送单明细状态
        /// </summary>
        public int AppStatus { get; set; }

        public string UrlSignImage { get; set; }

        /// <summary>
        /// 是否允许部分签收(百城当日达,到付)
        /// </summary>
        public bool IsPartialSignAllowed { get; set; }

    }

    /// <summary>
    /// wcfmodel配送单item->商品
    /// </summary>
    /// <remarks>2013-06-21 黄伟 创建</remarks>
    public class Item
    {
        /// <summary>
        /// 订单明细编号
        /// </summary>
        public int OrderItemSysNo { get; set; }

        /// <summary>
        /// 商品签收数量
        /// </summary>
        public int SignQuantity { get; set; }

        /// <summary>
        /// 商品号
        /// </summary>
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public double OriginalPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 出库单明细编号
        /// </summary>
        public int StockItemSysNo { get; set; }


    }

}
