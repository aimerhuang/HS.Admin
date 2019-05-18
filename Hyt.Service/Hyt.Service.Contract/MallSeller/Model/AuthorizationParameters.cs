using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Service.Contract.MallSeller.Model
{
    /// <summary>
    /// 授权参数
    /// </summary>
    /// <remarks>2014-1-1 陶辉 创建</remarks>
    public class AuthorizationParameters
    {
        /// <summary>
        /// 商城类型
        /// </summary>
        public int MallType { get; set; }

        /// <summary>
        /// 授权信息
        /// </summary>
        public string AuthorizationCode { get; set; }

        /// <summary>
        /// 店铺账号
        /// </summary>
        public string ShopAccount { get; set; }
    }
}
