using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 苏宁配置
    /// </summary>
    /// <remarks>2017-09-07 黄杰 创建</remarks>
    public class SUNINGConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SUNINGConfig()
        {

        }

        /// <summary>
        /// 苏宁API调用入口
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// 苏宁开放平台应用Appkey
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 苏宁开放平台应用AppSecret
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
        public string SUNINGCallBack { get; set; }

        /// <summary>
        /// 授权码换取登录令牌地址
        /// </summary>
        public string AccessTokenUrl { get; set; }

    }
}
