using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{

    public class RongEgouConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RongEgouConfig()
        {

        }

        /// <summary>
        /// Api接口地址
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// 订单列表查询接口代码
        /// </summary>
        public string OrderListSelect { get; set; }

        /// <summary>
        /// 订单详情查询接口代码
        /// </summary>
        public string OrderSelect { get; set; }

        /// <summary>
        /// 订单发货通知接口代码
        /// </summary>
        public string OrderDelivery { get; set; }

        /// <summary>
        /// app_key
        /// </summary>
        public string app_key { get; set; }

        /// <summary>
        /// app_secret
        /// </summary>
        public string app_secret { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string auth_code { get; set; }
    }
}
