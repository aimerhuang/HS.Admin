using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 国美配置
    /// </summary>
    /// <remarks>2017-08-29 黄杰 创建</remarks>
    public class GuoMeiConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GuoMeiConfig()
        {

        }

        /// <summary>
        /// 国美API调用入口
        /// </summary>
        public string ApiUrl { get; set; }


        /// <summary>
        /// 国美开放平台应用Appkey
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 国美开放平台应用AppSecret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 国美开放平台应用AccessToken
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 用户授权获取授权码地址
        /// </summary>
        public string AuthorizeUrl { get; set; }

        /// <summary>
        /// 国美授权回调地址
        /// </summary>
        public string GuoMeiCallBack { get; set; }

        /// <summary>
        /// 授权码换取登录令牌地址
        /// </summary>
        public string AccessTokenUrl { get; set; }
    }
}
