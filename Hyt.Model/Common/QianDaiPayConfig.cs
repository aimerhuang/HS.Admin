using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{

    /// <summary>
    /// 钱袋支付控制
    /// </summary>
    public class QianDaiPayConfig
    {
        #region 属性
        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public string Partner
        {
            get;
            set;
        }



        /// <summary>
        /// 获取或设交易安全校验码
        /// </summary>
        public  string Key
        {
            get;
            set;
        }

        /// <summary>
        /// 获取字符编码格式
        /// </summary>
        public  string Charset_name
        {
            get;
            set;
        }

        /// <summary>
        /// 获取签名方式
        /// </summary>
        public  string Sign_type
        {
            get;
            set;
        }

        /// <summary>
        /// 获取签名方式
        /// </summary>
        public  string Server_url
        {
            get;
            set;
        }
        /// <summary>
        /// 海关类型
        /// </summary>
        public string EbcType { get; set; }
        /// <summary>
        /// 海关编码
        /// </summary>
        public string EbcCode { get; set; }
        /// <summary>
        /// 海关名称
        /// </summary>
        public string EbcName { get; set; }
        /// <summary>
        /// 商检类型
        /// </summary>
        public string ICPType { get; set; }
        /// <summary>
        /// 商检编码
        /// </summary>
        public string ICPCode { get; set; }
        /// <summary>
        /// 商检名称
        /// </summary>
        public string ICPName { get; set; }

        public string Server_BackUrl { get; set; }

        #endregion
    }
}
