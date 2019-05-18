using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 有赞配置
    /// </summary>
    /// <remarks>2016-6-11 杨浩 创建</remarks>
    public class YouZanConfig
    {
        /// <summary>
        /// API调用入口
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// 开放平台应用appkeyURL
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 开放平台应用appsecret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 用户授权获取授权码地址
        /// </summary>
        public string AuthorizeUrl { get; set; }

        /// <summary>
        /// 授权回调地址
        /// </summary>
        public string YouZanCallBack { get; set; }

        /// <summary>
        /// 升舱订单标识
        /// </summary>
        public int SellerFlag { get; set; }

        /// <summary>
        /// 已升舱的订单标识旗
        /// </summary>
        public int ExcludeFlag { get; set; }
    }
}
