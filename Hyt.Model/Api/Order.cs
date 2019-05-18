using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.Api
{
    /// <summary>
    /// 订单
    /// </summary>
    /// <remarks>2016-9-8 杨浩 创建</remarks>
    [Serializable]
    public class Order
    {
        
        /// <summary>
        /// 订购商品列表
        /// </summary>
        public IList<OrderItem> OrderItemList { get; set; }
        /// <summary>
        /// 关联会员
        /// </summary>
        public CrCustomer Customer { get; set; }

        /// <summary>
        /// 关联收货地址
        /// </summary>
        public SoReceiveAddress ReceiveAddress { get; set; }
  
        /// <summary>
        /// 订单发票信息
        /// </summary>
        /// <remarks>2013-06-21 朱成果 创建</remarks>
        public FnInvoice OrderInvoice { get; set; }
        /// <summary>
        /// 在线支付信息
        /// </summary>
        /// <remarks>2016-9-7 杨浩 添加</remarks>
        public FnOnlinePayment OnlinePayment { get; set; }


        #region 订单属性

        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 订单收货地址系统编号
        /// </summary>
        [Description("订单收货地址系统编号")]
        public int ReceiveAddressSysNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        [Description("事务编号")]
        public string TransactionSysNo { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>
        [Description("会员编号")]
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 客户等级编号
        /// </summary>
        [Description("客户等级编号")]
        public int LevelSysNo { get; set; }
        /// <summary>
        /// 默认仓库编号
        /// </summary>
        [Description("默认仓库编号")]
        public int DefaultWarehouseSysNo { get; set; }
        /// <summary>
        /// 配送配送方式编号
        /// </summary>
        [Description("配送配送方式编号")]
        public int DeliveryTypeSysNo { get; set; }
        /// <summary>
        /// 支付方式编号
        /// </summary>
        [Description("支付方式编号")]
        public int PayTypeSysNo { get; set; }
        /// <summary>
        /// 商品销售价合计(明细销售单价*数量,之和.优惠前金额)
        /// </summary>
        [Description("商品销售价合计(明细销售单价*数量,之和.优惠前金额)")]
        public decimal ProductAmount { get; set; }
        /// <summary>
        /// 商品折扣金额(明细折扣金额之和)
        /// </summary>
        [Description("商品折扣金额(明细折扣金额之和)")]
        public decimal ProductDiscountAmount { get; set; }
        /// <summary>
        /// 商品调价金额合计(明细调价金额之和)
        /// </summary>
        [Description("商品调价金额合计(明细调价金额之和)")]
        public decimal ProductChangeAmount { get; set; }
        /// <summary>
        /// 运费折扣金额
        /// </summary>
        [Description("运费折扣金额")]
        public decimal FreightDiscountAmount { get; set; }
        /// <summary>
        /// 运费调价金额(正负值)
        /// </summary>
        [Description("运费调价金额(正负值)")]
        public decimal FreightChangeAmount { get; set; }
        /// <summary>
        /// 已使用促销系统编号(多个促销分号分隔)
        /// </summary>
        [Description("已使用促销系统编号(多个促销分号分隔)")]
        public string UsedPromotions { get; set; }
        /// <summary>
        /// 销售单总金额(商品销售价合计+运费-商品折扣金额-运费
        /// </summary>
        [Description("销售单总金额(商品销售价合计+运费-商品折扣金额-运费")]
        public decimal OrderAmount { get; set; }
        /// <summary>
        /// 运费金额(折扣前金额)
        /// </summary>
        [Description("运费金额(折扣前金额)")]
        public decimal FreightAmount { get; set; }
        /// <summary>
        /// 订单折扣金额(订单折扣金额，不包含商品折扣金额)
        /// </summary>
        [Description("订单折扣金额(订单折扣金额，不包含商品折扣金额)")]
        public decimal OrderDiscountAmount { get; set; }
        /// <summary>
        /// 优惠券金额(多张优惠券合计)
        /// </summary>
        [Description("优惠券金额(多张优惠券合计)")]
        public decimal CouponAmount { get; set; }
        /// <summary>
        /// 总金额中现金支付部分
        /// </summary>
        [Description("总金额中现金支付部分")]
        public decimal CashPay { get; set; }
        /// <summary>
        /// 总金额中惠源币支付部分
        /// </summary>
        [Description("总金额中惠源币支付部分")]
        public decimal CoinPay { get; set; }
        /// <summary>
        /// 发票系统编号
        /// </summary>
        [Description("发票系统编号")]
        public int InvoiceSysNo { get; set; }
        /// <summary>
        /// 下单来源：PC网站（10）、信营全球购B2B2C3G网站（15）
        /// </summary>
        [Description("下单来源：PC网站（10）、信营全球购B2B2C3G网站（15）")]
        public int OrderSource { get; set; }
        /// <summary>
        /// 下单来源编号
        /// </summary>
        [Description("下单来源编号")]
        public int OrderSourceSysNo { get; set; }
        /// <summary>
        /// 销售方式：普通订单（10）、调价订单（40）、经销订单
        /// </summary>
        [Description("销售方式：普通订单（10）、调价订单（40）、经销订单")]
        public int SalesType { get; set; }
        /// <summary>
        /// 销售方式编号
        /// </summary>
        [Description("销售方式编号")]
        public int SalesSysNo { get; set; }
        /// <summary>
        /// 此销售单对用户是否隐藏：是（1）、否（0）
        /// </summary>
        [Description("此销售单对用户是否隐藏：是（1）、否（0）")]
        public int IsHiddenToCustomer { get; set; }
        /// <summary>
        /// 支付状态：未支付（10）、已支付（20）、支付异常（30
        /// </summary>
        [Description("支付状态：未支付（10）、已支付（20）、支付异常（30")]
        public int PayStatus { get; set; }
        /// <summary>
        /// 客户留言
        /// </summary>
        [Description("客户留言")]
        public string CustomerMessage { get; set; }
        /// <summary>
        /// 对内备注
        /// </summary>
        [Description("对内备注")]
        public string InternalRemarks { get; set; }
        /// <summary>
        /// 配送备注
        /// </summary>
        [Description("配送备注")]
        public string DeliveryRemarks { get; set; }
        /// <summary>
        /// 配送时间段
        /// </summary>
        [Description("配送时间段")]
        public string DeliveryTime { get; set; }
        /// <summary>
        /// 配送前是否联系：是（1）、否（0）
        /// </summary>
        [Description("配送前是否联系：是（1）、否（0）")]
        public int ContactBeforeDelivery { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }
        /// <summary>
        /// 下单人编号（系统用户）
        /// </summary>
        [Description("下单人编号（系统用户）")]
        public int OrderCreatorSysNo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 销售单审核人编号
        /// </summary>
        [Description("销售单审核人编号")]
        public int AuditorSysNo { get; set; }
        /// <summary>
        /// 销售单审核时间
        /// </summary>
        [Description("销售单审核时间")]
        public DateTime AuditorDate { get; set; }
        /// <summary>
        /// 销售单作废人类型:前台用户(10),后台用户(20)
        /// </summary>
        [Description("销售单作废人类型:前台用户(10),后台用户(20)")]
        public int CancelUserType { get; set; }
        /// <summary>
        /// 销售单作废人编号
        /// </summary>
        [Description("销售单作废人编号")]
        public int CancelUserSysNo { get; set; }
        /// <summary>
        /// 销售单作废时间
        /// </summary>
        [Description("销售单作废时间")]
        public DateTime CancelDate { get; set; }
        /// <summary>
        /// 前台显示状态
        /// </summary>
        /// <remarks>
        /// 修改OnlineStatus返回Status对应的文本值 特殊修改   OnlineStatus完成没有用了
        /// </remarks>
        [Description("前台显示状态")]
        public string OnlineStatus
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_onlineStatus))
                {
                    switch (Status)
                    {
                        case 10:
                            return "待审核";
                        case 20:
                            return "待支付";
                        case 30:
                            return "待创建出库单";
                        case 40:
                            return "部分创建出库单";
                        case 50:
                            return "已创建出库单";
                        case 100:
                            return "已完成";
                        case -10:
                            return "作废";
                        default:
                            return "未知状态";
                    }
                }
                else
                {
                    return _onlineStatus;
                }
                //状态：待审核（10）、待支付（20）、待创建出库单（30）、部分创建出库单（40）、已创建出库单（50）、已完成（100）、作废（－10）

            }
            set
            {
                _onlineStatus = value;
            }
        }
        private string _onlineStatus = string.Empty;

        /// <summary>
        /// 状态：待审核（10）、待支付（20）、待创建出库单（30
        /// </summary>
        [Description("状态：待审核（10）、待支付（20）、待创建出库单（30")]
        public int Status { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Description("最后更新时间")]
        public DateTime LastUpdateDate { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        [Description("时间戳")]
        public DateTime Stamp { get; set; }

        /// <summary>
        /// 图片标识
        /// </summary>
        [Description("图片标识")]
        public string ImgFlag { get; set; }

        /// <summary>
        /// 推送状态：未推送（0）、已推送（1
        /// </summary>
        [Description("推送状态：未推送（0）、已推送（1）")]
        public int SendStatus { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [Description("订单编号")]
        public string OrderNo { get; set; }
        /// <summary>
        /// 分销商系统编号
        /// </summary>
        [Description("分销商系统编号")]
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 行邮税
        /// </summary>
        [Description("行邮税")]
        public decimal TaxFee { get; set; }
        /// <summary>
        /// 支付信息系海关推送状态
        /// </summary>
        [Description("支付信息系海关推送状态")]
        public int CustomsPayStatus { get; set; }
        /// <summary>
        /// 订单商检推送状态
        /// </summary>
        [Description("订单商检推送状态")]
        public int TradeCheckStatus { get; set; }
        /// <summary>
        /// 订单海关推送状态
        /// </summary>
        [Description("订单海关推送状态")]
        public int CustomsStatus { get; set; }

        /// <summary>
        /// 返利比例（记录当前的利润返利比例）
        /// </summary>
        [Description("返利比例")]
        public string RebateRtio { get; set; }
        /// <summary>
        /// 操作费
        /// </summary>
        [Description("操作费")]
        public decimal OperatFee { get; set; }
        /// <summary>
        /// 广州机场商检订单推送状态
        /// </summary>
        [Description("广州机场商检订单推送状态")]
        public int GZJCStatus { get; set; }
        /// <summary>
        /// 订单南沙商检推送状态
        /// </summary>
        [Description("订单南沙商检推送状态")]
        public int NsStatus { get; set; }
        /// <summary>
        /// 跨境物流推送状态
        /// </summary>
        [Description("跨境物流推送状态")]
        public int CBLogisticsSendStatus { get; set; }
        #endregion
    }
}
