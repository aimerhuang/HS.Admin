using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{

    /// <summary>
    /// 分销配送单打印实体
    /// </summary>
    /// <remarks>
    /// 2013-09-13 郑荣华 创建
    /// </remarks>
    public class PrtDsDelivery : LgDelivery
    {
        /// <summary>
        /// 明细列表
        /// </summary>
        public IList<PrtDsSubDelivery> List;

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WareHouseName { get; set; }

        /// <summary>
        /// 配送方式父编号（主表来）
        /// </summary>
        public int ParentSysNo { get; set; }

     
    }

    /// <summary>
    /// 分销配送单打印明细实体
    /// </summary>
    /// <remarks>
    /// 2013-09-13 郑荣华 创建
    /// </remarks>
    public class PrtDsSubDelivery : LgDeliveryItem
    {
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 收货地区编号
        /// </summary>
        public int AreaSysNo { get; set; }

        /// <summary>
        /// 地区全称（区,市,省）
        /// </summary>
        public string AreaAllName
        {
            get { return _areaAllName; }
            set { _areaAllName = Reverse(value ?? ""); }
        }

        /// <summary>
        /// （区,市,省）变为（省市区）
        /// </summary>
        /// <returns></returns>
        private static string Reverse(string x)
        {
            var t = x.Split(',').ToList();
            var r = "";
            for (var i = t.Count - 1; i >= 0; i--)
            {
                r += t[i];
            }
            return r;
        }

        private string _areaAllName;

        /// <summary>
        /// 收货街道地址
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// 收货人固定电话号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public int OrderSysNo { get; set; }

        /// <summary>
        /// 支付名称
        /// </summary>
        public string PaymentName { get; set; }

        /// <summary>
        /// 店铺名称，本公司用 商城 分销商实际是list，取第一条
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 来源单类型名称（取件单，出库单（升舱订单、商城订单））
        /// </summary>
        public string NoteTypeName { get; set; }

        /// <summary>
        /// 配送方式名称(从出库单来)
        /// </summary>
        public string DeliveryTypeName { get; set; }

        /// <summary>
        /// 商品数量合计
        /// </summary>
        public int ProductQuantityCount
        {
            get
            {
                return PdList != null ? PdList.Sum(item => item.ProductQuantity) : 0;
            }
        }

        /// <summary>
        /// 实际销售金额合计
        /// </summary>     
        public decimal RealSalesAmountCount
        {
            get
            {
                return PdList != null ? PdList.Sum(item => item.RealSalesAmount) : 0;
            }
        }

        /// <summary>
        /// 出库单或取件单商品明细列表（共用）
        /// </summary>
        public IList<PrtDsWhStockOutItem> PdList;

        /// <summary>
        /// 出库单备注
        /// </summary>
        public string StockOutRemarks { get; set; }

        /// <summary>
        /// 订单运费金额
        /// </summary>
        public decimal FreightAmount { get; set; }
    }
}
