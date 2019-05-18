using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 拍拍升舱配置
    /// </summary>
    /// <remarks>2017-08-23 黄杰 重构</remarks>
    public class YihaodianConfig
    {
        /// <summary>
        /// 一号店API调用入口
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// 一号店开发平台应用sessionKey
        /// </summary>
        public string SessionKey { get; set; }

        /// <summary>
        /// 一号店开放平台应用appkeyURL
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 一号店开放平台应用appsecret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 用户授权获取授权码地址
        /// </summary>
        public string AuthorizeUrl { get; set; }

        /// <summary>
        /// 授权回调地址
        /// </summary>
        public string YihaodianCallBack { get; set; }

        /// <summary>
        /// 一号店升舱订单标识
        /// </summary>
        public int DeliverySupplierId { get; set; }

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
