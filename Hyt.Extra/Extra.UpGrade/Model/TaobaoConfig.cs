using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 淘宝配置
    /// </summary>
    /// <remarks>2013-12-31 陶辉 创建</remarks>
    public class TaobaoConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TaobaoConfig()
        {

        }

        /// <summary>
        /// 淘宝API调用入口
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// 淘宝开放平台应用appkeyURL
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 淘宝开放平台应用appsecret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 用户授权获取授权码地址
        /// </summary>
        public string AuthorizeUrl { get; set; }

        /// <summary>
        /// 授权回调地址
        /// </summary>
        public string TaobaoCallBack { get; set; }

        /// <summary>
        /// 淘宝升舱订单标识
        /// </summary>
        public int SellerFlag { get; set; }

        /// <summary>
        /// 已升舱的订单标识旗
        /// </summary>
        public int ExcludeFlag { get; set; }

        /// <summary>
        /// 授权码换取登录令牌地址
        /// </summary>
        public string AccessTokenUrl { get; set; }
    }
}
