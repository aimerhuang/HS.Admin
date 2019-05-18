using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 授权参数
    /// </summary>
    /// <remarks>2014-1-1 杨浩 创建</remarks>
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
        /// <summary>
        /// 应用
        /// </summary>
        public DsDealerApp DealerApp { get; set; }
        /// <summary>
        /// 商城详情
        /// </summary>
        public DsDealerMall DealerMall { get; set; }
    }
}
