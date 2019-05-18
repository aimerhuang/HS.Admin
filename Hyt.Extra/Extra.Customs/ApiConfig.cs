using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Customs
{
    public static class ApiConfig
    {
        /// <summary>
        /// 广州海关报文采用标准的AES加密，加密的密钥 Key
        /// </summary>
        public static string GuangZhouCustomsKey = "MYgGnQE2+DAS973vd1DFHg==";
        /// <summary>
        /// 报文类型 880020-电子订单
        /// </summary>
        public static string MessageType = "880020";
        /// <summary>
        /// 信营企业备案号
        /// </summary>
        public static string XinYinSenderID = "IE150723865142";

        /// <summary>
        /// 信营海关FTP地址
        /// </summary>
        public static string FtpUrl = "ftp://210.21.48.7:2312/";

        /// <summary>
        /// 信营海关FTP地址用户名
        /// </summary>
        public static string FtpUserName = "XYMY";

        /// <summary>
        /// 信营海关FTP地址密码
        /// </summary>
        public static string FtpPassword = "z233L7PK";

        /// <summary>
        /// 版本号
        /// </summary>
        public static string Version = "1.0";
    }
}
