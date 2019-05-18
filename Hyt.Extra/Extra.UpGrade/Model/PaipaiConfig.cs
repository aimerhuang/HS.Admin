using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 拍拍配置
    /// </summary>
    /// <remarks>2013-12-31 陶辉 创建</remarks>
    public class PaipaiConfig
    {
        /// <summary>
        /// 腾讯电商平台应用appkey
        /// </summary>
        public string AppOAuthID { get; set; }

        /// <summary>
        /// 腾讯电商平台应用appsecret
        /// </summary>
        public string SecretOAuthKey { get; set; }

        /// <summary>
        /// 授权地址
        /// </summary>
        public string AuthorizeUrl { get; set; }

        /// <summary>
        /// 授权回调地址
        /// </summary>
        public string PopCallBack { get; set; }

        /// <summary>
        /// 订单旗帜
        /// </summary>
        public string DealNoteType { get; set; }
    }
}
