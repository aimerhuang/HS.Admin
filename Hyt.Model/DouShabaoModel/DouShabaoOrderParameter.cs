using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.DouShabaoModel
{
    /// <summary>
    /// 订单参数
    /// </summary>
    /// <remarks>2017-6-12 罗勤尧 创建</remarks>
    public class DouShabaoOrderParameter
    {
        #region 传入参数
        /// <summary>
        /// 商户订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 运输方式（直邮/转运）
        /// </summary>
        public string ExpressChannel { get; set; }
        /// <summary>
        /// 保单总价
        /// </summary>
        public string TotalAmount { get; set; }
        /// <summary>
        /// 订单总重量（单位：kg）
        /// </summary>
        public double TotalWeight { get; set; }
        /// <summary>
        /// 买家名称
        /// </summary>
        public string BuyerName { get; set; }
        /// <summary>
        /// 买家手机号
        /// </summary>
        public string BuyerMobile { get; set; }
        /// <summary>
        /// 买家身份证号码
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 物流单号
        /// </summary>
        public string ExpressNo { get; set; }
        /// <summary>
        /// 物流时间
        /// </summary>
        public string ExpressTime { get; set; }
        /// <summary>
        /// 转运路线（包裹运输所走线路，如：EMS清关路线）
        /// </summary>
        public string TransitLine { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceiptAddress { get; set; }
        /// <summary>
        /// 发货地址（直邮方式指商家发货地址；转运方式指仓库地址）
        /// </summary>
        public string ShippingAddress { get; set; }
        /// <summary>
        /// 订单时间
        /// </summary>
        public DateTime OrderTime { get; set; }
        /// <summary>
        /// 是否加固（0/加固 1/未加固）
        /// </summary>
        public int IsReinforce { get; set; }
        /// <summary>
        /// 时效，用户从下单到收货所需时间（单位/天）
        /// </summary>
        public int Prescription { get; set; }
        /// <summary>
        /// 商户标识Source
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 接口接入签名
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 海淘网站购物订单号
        /// </summary>
        public string PurchasOrderNo { get; set; }
        /// <summary>
        /// 海淘购物网站
        /// </summary>
        public string ShoppingSite { get; set; }
        /// <summary>
        /// 海淘商品总价格
        /// </summary>
        public string PurchasOrderAmount { get; set; }
        /// <summary>
        /// 海淘购物下单时间
        /// </summary>
        public string PurchasOrderTime { get; set; }
        /// <summary>
        /// 重量单位
        /// </summary>
        public string SalesMeasurementUnit { get; set; }
        /// <summary>
        /// 海淘购买商品 list
        /// </summary>
        public List<ProductList> Product { get; set; }
        /// <summary>
        /// 投保商品 list
        /// </summary>
        public List<InsuranceProductList> InsuranceProduct { get; set; }

        #endregion
    }
}
