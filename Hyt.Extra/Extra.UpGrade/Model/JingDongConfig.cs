using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 京东配置
    /// </summary>
    /// <remarks>2017-08-10 黄杰 创建</remarks>
    public class JingDongConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public JingDongConfig()
        {

        }

        /// <summary>
        /// 京东API调用入口
        /// </summary>
        public string ApiUrl { get; set; }


        /// <summary>
        /// 京东开放平台应用Appkey
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 京东开放平台应用AppSecret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 京东开放平台应用AccessToken
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 用户授权获取授权码地址
        /// </summary>
        public string AuthorizeUrl { get; set; }

        /// <summary>
        /// 京东授权回调地址
        /// </summary>
        public string JingDongCallBack { get; set; }

        /// <summary>
        /// 京东升舱订单标识
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
