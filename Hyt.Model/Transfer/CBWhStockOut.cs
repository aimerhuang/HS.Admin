using System;
using System.Collections.Generic;
using System.Linq;

namespace Hyt.Model
{
    /// <summary>
    /// 出库单实体
    /// </summary>
    /// <remarks> 2013-06-18 周瑜 创建</remarks>
    [Serializable]
    public class CBWhStockOut: WhStockOut
    {

        #region 扩展属性

        /// <summary>
        /// 出库时已扫描的商品明细
        /// </summary>
        public IList<WhStockOutItem> ScanedItems { get; set; }

        /// <summary>
        /// 会员系统编号
        /// </summary>
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 订单创建日期
        /// </summary>
        public DateTime SoCreateDate { get; set; }

        /// <summary>
        /// 订单销售类型
        /// </summary>
        public int SalesType { get; set; }

        /// <summary>
        /// 订单审核日期
        /// </summary>
        public DateTime? AuditorDate { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal FreightAmount { get; set; }

        /// <summary>
        /// 配送方式名称
        /// </summary>
        public string DeliveryTypeName { get; set; }

        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string PaymentName { get; set; }

        /// <summary>
        /// 销售单对内备注
        /// </summary>
        public string SoInternalRemarks { get; set; }

        /// <summary>
        /// 客户留言
        /// </summary>
        public string SoCustomerMessage { get; set; }

        /// <summary>
        /// 是否是第三方快递 0: 否 1:是 
        /// </summary>
        public int IsThirdpartyExpress { get; set; }

        ///// <summary>
        ///// 省
        ///// </summary>
        //public string Province { get; set; }

        ///// <summary>
        ///// 市
        ///// </summary>
        //public string City { get; set; }

        /// <summary>
        /// 地区全称
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 详细收货地址
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 联系手机
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        public string SoRemarks { get; set; }

        /// <summary>
        /// 发票号码
        /// </summary>
        public string InvoiceCode { get; set; }

        /// <summary>
        /// 发票编号
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 发票类型
        /// </summary>
        public int InvoiceTypeSysNo { get; set; }

        /// <summary>
        /// 发票类型名称
        /// </summary>
        public string InvoiceTypeName { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        public string InvoiceTitle { get; set; }

        /// <summary>
        /// 发票金额
        /// </summary>
        public decimal InvoiceAmount { get; set; }

        /// <summary>
        /// 发票备注
        /// </summary>
        public string InvoiceRemarks { get; set; }

        /// <summary>
        /// 发票状态 待开票(10),已开票(20),作废(-10)
        /// </summary>
        public int InvoiceStatus { get; set; }

        public int GroupId { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        /// <remarks>2014-03-07 唐文均 新增 经HYD同意BUG4468</remarks>
        public string OrderSource { get; set; }

        /// <summary>
        /// 此字段仅用于分页，不参与查询与显示
        /// </summary>
        public int FLUENTDATA_ROWNUMBER { get; set; }
        #endregion

        /// <summary>
        /// 重载Equals，满足Equals的条件：
        /// 1. Items数量相等
        /// 2. Items里面的ProductSysNo相等
        /// 3. 每个ProductSysNo的Quantity相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var master = obj as CBWhStockOut;
            if (master == null)
            {
                return false;
            }
            if (master.Items.Count != Items.Count)
            {
                return false;
            }
            var itemFound = from a in master.Items
                            join b in Items on new {a.ProductSysNo, a.ProductQuantity} equals
                                new {b.ProductSysNo, b.ProductQuantity}
                            select a;
            if (itemFound.Count() != master.Items.Count)
            {
                return false;
            }
            return true;
        }
    }
}