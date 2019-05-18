using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 网银支付配置
    /// </summary>
    /// <remarks>2014-1-20 黄波 创建</remarks>
    public class PayConfig : ConfigBase
    {
        #region 易宝
        /// <summary>
        ///易宝支付 商户号
        /// </summary>
        public string EhkingMerhantId { get; set; }
        /// <summary>
        ///易宝支付 秘钥
        /// </summary>
        public string EhkingKey { get; set; }
        /// <summary>
        /// 易宝支付网关地址
        /// </summary>
        public string EhkingGateway { get; set; }

        /// <summary>
        ///异步对账地址
        /// <summary>
        public string EhkingAsyncReturnUrl { get; set; }

        /// <summary>
        /// 同步回调页面URL
        /// </summary>
        public string EhkingSyncReturnUrl { get; set; }
        /// <summary>
        /// 海关报关异步回执
        /// </summary>
        public string EhkingCustomsAsyncUrl { get; set; }
        /// <summary>
        /// 报关网关
        /// </summary>
        public string EhkingNodeAuthorizationUrl { get; set; }
        #endregion

        #region 通联支付
        /// <summary>
        /// 报关网关
        /// </summary>
        public string TLPayCustomsUrl { get; set; }
        /// <summary>
        /// 电商标识
        /// </summary>
        public string TLPaySenderID { get; set; }
        /// <summary>
        /// 报文签名密钥
        /// </summary>
        public string TLPaySignKey { get; set; }
        /// <summary>
        /// 海关回执通知地址
        /// </summary>
        public string TLPayReceiptUrl { get; set; }
        #endregion

        /// <summary>
        /// 合作伙伴ID
        /// </summary>
        public string AliPayPartnerID { get; set; }

        ///异步对账地址
        public string AliPayAsyncReturnUrl { get; set; }

        /// <summary>
        /// 同步回调页面URL
        /// </summary>
        public string AliPaySyncReturnUrl { get; set; }

        /// <summary>
        /// 卖家支付宝账号
        /// </summary>
        public string AliPaySellerEmail { get; set; }

        /// <summary>
        /// 支付宝网关地址
        /// </summary>
        public string AliPayGateway { get; set; }

        /// <summary>
        /// 安全校验码
        /// </summary>
        public string AliPaykey { get; set; }

        //网银在线参数
        /// <summary>
        /// 商户号
        /// </summary>
        public string ChinaBankMID { get; set; }

        /// <summary>
        /// 异步对账地址
        /// </summary>
        public string ChinaBankAsyncReturnUrl { get; set; }

        /// <summary>
        /// 同步回调页面URL
        /// </summary>
        public string ChinaBankSyncReturnUrl { get; set; }

        /// <summary>
        /// 网银在线网关地址
        /// </summary>
        public string ChinaBankGateway { get; set; }

        /// <summary>
        /// 网银在线来源地址,请求本网站所用网址
        /// </summary>
        public string ChinaBankFromUrl { get; set; }

        /// <summary>
        /// 安全校验码MD5
        /// </summary>
        public string ChinaBankkey { get; set; }

        /// <summary>
        /// 手机版异步对账地址
        /// </summary>
        /// <remarks>2015-10-29 陈海裕 创建</remarks>
        public string AliPayAsyncReturnUrl_App { get; set; }

        /// <summary>
        /// 手机版同步对账地址
        /// </summary>
        /// <remarks>2015-10-29 陈海裕 创建</remarks>
        public string AliPaySyncReturnUrl_App { get; set; }

        /// <summary>
        /// 手机版操作中断回调地址
        /// </summary>
        public string MerchantUrl_App { get; set; }

        /// <summary>
        /// 手机版网关
        /// </summary>
        public string AliPayGateway_App { get; set; }

        #region 微信支付
        /// <summary>
        /// 公众账号ID 
        /// </summary>
        public string WxAppid { get; set; }
        /// <summary>
        /// 商户号 
        /// </summary>
        public string WxMchId { get; set; }
        /// <summary>
        /// 商户支付密钥，参考开户邮件设置（必须配置）
        /// </summary>
        public string WxKey { get; set; }
        /// <summary>
        /// 公众帐号secert（仅JSAPI支付的时候需要配置）
        /// </summary>
        public string WxAppSecret { get; set; }
        /// <summary>
        /// 支付结果通知回调url，用于商户接收支付结果
        /// </summary>
        public string WxNotifyUrl { get; set; }

        #endregion
    }
}
