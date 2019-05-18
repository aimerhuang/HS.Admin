using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hyt.Model;

namespace Hyt.Admin.Models
{
    /// <summary>
    /// 7.退换货单显示实体
    /// </summary>
    /// <remarks>2013－07-11 黄志勇 创建</remarks>
    public class ReturnView
    {
        /// <summary>
        /// 退换货单编号
        /// 余勇添加
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 提示消息
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 退换货出库单列表
        /// </summary>
        public List<ReturnEditOutStore> ReturnEditOutStore { get; set; }

        ///<summary>
        /// 合计退款金额
        /// </summary>
        public decimal RefundAmount { get; set; }
        /// <summary>
        /// 退款方式
        /// </summary>
        public int RefundType { get; set; }
        /// <summary>
        /// 退款方式名称
        /// </summary>
        public string RefundTypeName { get; set; }
        /// <summary>
        /// 退款开户行
        /// </summary>
        public string RefundBank { get; set; }
        /// <summary>
        /// 退款开户帐号
        /// </summary>
        public string RefundAccount { get; set; }
        /// <summary>
        /// 退款开户人
        /// </summary>
        public string RefundAccountName { get; set; }

        /// <summary>
        /// 商品入库仓库
        /// </summary>
        public string WhStockInName { get; set; }

        /// <summary>
        /// 取件方式
        /// </summary>
        public string LgPickupTypeName { get; set; }
        /// <summary>
        /// 取件地址
        /// </summary>
        public SoReceiveAddress PickUpAddress { get; set; }
        /// <summary>
        /// 会员收货地址
        /// </summary>
        public SoReceiveAddress SoReceiveAddress { get; set; }
        /// <summary>
        /// 取件地址全名
        /// 余勇添加
        /// </summary>
        public string PickUpAddressFullName { get; set; }
        /// <summary>
        /// 收货地址全名
        /// 余勇添加
        /// </summary>
        public string ReceiveAddressFullName { get; set; }
        /// <summary>
        /// 预约时间（余勇添加）
        /// </summary>
        public string PickUpTime { get; set; }
        /// <summary>
        /// 是否需要取回发票:需要/不需要（余勇添加）
        /// </summary>
        public string IsPickUpInvoice { get; set; }
        /// <summary>
        /// 会员备注
        /// </summary>
        public string RMARemark { get; set; }
        /// <summary>
        /// 对内备注
        /// </summary>
        public string InternalRemark { get; set; }

        /// <summary>
        /// 退换单处理部门:客服(10),门店(20)
        /// </summary>
        public int HandleDepartment { get; set; }
        /// <summary>
        /// 发票扣款金额
        /// </summary>
        public decimal DeductedInvoiceAmount { get; set; }

        /// <summary>
        /// 实退金额
        /// </summary>
        public decimal ActualRefundAmount { get; set; }

        /// <summary>
        /// 应退金额
        /// </summary>
        public decimal OrginAmount { get; set; }

        /// <summary>
        /// 应退惠源币
        /// </summary>
        public decimal OrginCoin { get; set; }

        /// <summary>
        /// 应扣回积分
        /// </summary>
        public decimal OrginPoint { get; set; }

        /// <summary>
        /// 扣回优惠卷
        /// </summary>
        public decimal CouponAmount { get; set; }

        /// <summary>
        /// 实退惠源币
        /// </summary>
        public decimal RefundCoin { get; set; }

        /// <summary>
        /// 实扣回积分
        /// </summary>
        public decimal RefundPoint { get; set; }

        /// <summary>
        /// 积分现金补偿金额
        /// </summary>
        public decimal RedeemAmount { get; set; }

        /// <summary>
        /// 退换货图片 朱家宏 添加
        /// </summary>
        public List<RcReturnImage> RmaImages { get; set; }
    }
}