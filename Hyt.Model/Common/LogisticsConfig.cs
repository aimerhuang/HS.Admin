using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 物流配置
    /// </summary>
    /// <remarks>2016-3-8 杨浩 创建</remarks>
    public class LogisticsConfig : ConfigBase
    {
        /// <summary>
        /// 应用键
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 秘钥
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 口岸编码
        /// </summary>
        public string PortCode { get; set; }
        /// <summary>
        /// 平台备案号
        /// </summary>
        public string PlatformCode { get; set; }
        /// <summary>
        /// 企业商检备案号
        /// </summary>
        public string CIECode { get; set; }
        public string CIEName { get; set; }
        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 查询路径
        /// </summary>
        public string RequestUrl { get; set; }

        #region 有信达物流方式为顺丰时的配置（信营专用）

        /// <summary>
        /// 顺丰客户编码
        /// </summary>
        public string SFCustomerCode { get; set; }
        /// <summary>
        /// 顺丰Key
        /// </summary>
        public string SFKey { get; set; }
        /// <summary>
        /// 顺丰Token
        /// </summary>
        public string SFToken { get; set; }

        #endregion
    }


    /// <summary>
    /// 物流配置
    /// </summary>
    /// <remarks>2016-3-8 杨浩 创建</remarks>
    public class LogisticsConfig2 : ConfigBase
    {
        /// <summary>
        /// 应用键
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 秘钥
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 口岸编码
        /// </summary>
        public string PortCode { get; set; }
        /// <summary>
        /// 平台备案号
        /// </summary>
        public string PlatformCode { get; set; }
        /// <summary>
        /// 企业商检备案号
        /// </summary>
        public string CIECode { get; set; }
        public string CIEName { get; set; }
        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 查询路径
        /// </summary>
        public string RequestUrl { get; set; }

        public string Account { get; set; }
        public string Password { get; set; }

        #region 有信达物流方式为顺丰时的配置（信营专用）

        /// <summary>
        /// 顺丰客户编码
        /// </summary>
        public string SFCustomerCode { get; set; }
        /// <summary>
        /// 顺丰Key
        /// </summary>
        public string SFKey { get; set; }
        /// <summary>
        /// 顺丰Token
        /// </summary>
        public string SFToken { get; set; }

        #endregion
    }
}
