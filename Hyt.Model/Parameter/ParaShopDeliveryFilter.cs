using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 门店提货参数字段
    /// </summary>
    /// <remarks>2013-07-08 余勇 创建</remarks>
    #region 门店提货
    public class ParaShopDeliveryFilter
    {
        /// <summary>
        /// 出库单SysNo
        /// </summary>
        public int stockOutSysNo { get; set; }
        /// <summary>
        /// 发票类型
        /// </summary>
        public int invoiceType { get; set; }
        /// <summary>
        /// 发票代码
        /// </summary>
        public string invoiceCode { get; set; }
        /// <summary>
        /// 发票备注
        /// </summary>
        public string invoiceRemarks { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string invoiceTitle { get; set; }
        /// <summary>
        /// 付款类型
        /// </summary>
        public int payType{get;set;}
        
        /// <summary>
        /// 地区编号
        /// </summary>
        public int areaSysNo { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string streetAddress { get; set; }
        /// <summary>
        /// 转快递原因
        /// </summary>
        public string shipReson { get; set; }
        /// <summary>
        /// 会员留言
        /// </summary>
        public string customerMessage { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public int paymentType { get; set; }
        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal payMoney { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string pickUpCode { get; set; }

        /// <summary>
        /// 发票号码
        /// </summary>
        public string invoiceNo { get; set; }

        /// <summary>
        /// 刷卡流水号(收款单明细-交易凭证号)
        /// </summary>
        /// <remarks>2013-10-14 朱家宏 添加</remarks>
        public string VoucherNo { get; set; }

        /// <summary>
        /// EAS收款科目编码
        /// </summary>
        public string EasReceiptCode { get; set; }
    }
    #endregion
}
