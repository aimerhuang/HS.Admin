using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 支付宝海关
    /// </summary>
    /// <remarks>2015-10-27 王耀发 创建</remarks>
    public class AlipayCustomsConfig : ConfigBase
    {
        /// <summary>
        /// 支付宝网关地址（新）
        /// </summary>
        public string GATEWAY_NEW { get; set; }
        /// <summary>
        ///商户的私钥
        public string key { get; set; }

        /// <summary>
        /// 编码格式
        /// </summary>
        public string input_charset { get; set; }

        /// <summary>
        /// 签名方式
        /// </summary>
        public string sign_type { get; set; }

        /// <summary>
        /// 合作者身份ID
        /// </summary>
        public string partner { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string service { get; set; }

        /// <summary>
        /// 商户在海关备案的编号
        /// </summary>
        public string merchant_customs_code { get; set; }

        /// <summary>
        /// 商户在海关备案的名称
        /// </summary>
        public string merchant_customs_name { get; set; }

        /// <summary>
        /// 海关编号
        /// </summary>
        public string customs_place { get; set; }

        /// <summary>
        /// 商户在商检备案的编号
        /// </summary>
        public string merchant_commodity_code { get; set; }
        /// <summary>
        /// 商户在商检备案的名称
        /// </summary>
        public string merchant_commodity_name { get; set; }
        /// <summary>
        /// 商检编号
        /// </summary>
        public string commodity_place { get; set; }


    }
}
