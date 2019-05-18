using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 第三方登录令牌信息
    /// </summary>
    /// <remarks>2013-9-9 陶辉 创建</remarks>
    public class AccessTokenResult
    {
        /// <summary>
        /// 登录令牌
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 令牌过期时间
        /// </summary>
        public int Expiresin { get; set; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 授权账号
        /// </summary>
        public string UserNick { get; set; }

    }
}
