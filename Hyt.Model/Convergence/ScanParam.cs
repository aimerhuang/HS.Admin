using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Convergence
{
    /// <summary>
    /// 汇聚扫码支付接口
    /// </summary>
    /// <remarks>2017-12-20 廖移凤 创建 </remarks>
    public class ScanParam
    {
        /// <summary>
        /// 版本号 Y
        /// </summary>
        public string P0_Version { get; set; }
        /// <summary>
        /// 商户编号 Y
        /// </summary>
        public string  P1_MerchantNo { get; set; }
        /// <summary>
        /// 商户订单号 Y
        /// </summary>
        public string  P2_OrderNo { get; set; }
        /// <summary>
        /// 支付金额 Y 
        /// </summary>
        public double P3_Amount { get; set; }
        /// <summary>
        /// 币种 Y 1:人民币 
        /// </summary>
        public int P4_Cur { get; set; }
        /// <summary>
        /// 商品名称 Y
        /// </summary>
        public string P5_ProductName { get; set; }
       /// <summary>
       /// 商品描述 N
       /// </summary>
        public string P6_ProductDesc { get; set; }
       /// <summary>
        /// 公用回传参数 N
       /// </summary>
        public string P7_Mp { get; set; }
       /// <summary>
       /// 商户页面通知地址 N 处理结果页面跳转到商户网站里指定的 http 地址
       /// </summary>
        public string P8_ReturnUrl { get; set; }
       /// <summary>
        /// 服务器异步通知地址 Y 主动通知商户
       /// </summary>
        public string P9_NotifyUrl { get; set; }
        /// <summary>
        /// 交易类型 Y 
        /// </summary>
        public string Q1_FrpCode { get; set; }
       /// <summary>
       /// 银行商户编码   N
       /// </summary>
        public string Q2_MerchantBankCode { get; set; }
       /// <summary>
        /// 子商户号(保留    N
       /// </summary>
        public string Q3_SubMerchantNo { get; set; }
        /// <summary>
        /// 是否展示图片 N 1 表示输出图片
        /// </summary>
        public int Q4_IsShowPic { get; set; }
       /// <summary>
        /// 微信 Openid   N
       /// </summary>
        public string Q5_OpenId { get; set; }
       /// <summary>
        /// 付款码数字   N
       /// </summary>
        public string Q6_AuthCode { get; set; }
       /// <summary>
        /// APPID      N
       /// </summary>
        public string Q7_AppId { get; set; }
       /// <summary>
        /// 终端号     N
       /// </summary>
        public string Q8_TerminalNo { get; set; }
       /// <summary>
        /// 微信 H5 模式   N
       /// </summary>
        public string Q9_TransactionModel { get; set; }







       
    }
}
