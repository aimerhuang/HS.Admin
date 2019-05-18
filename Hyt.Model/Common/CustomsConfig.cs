using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 海关
    /// </summary>
    /// <remarks>
    /// 2015-10-27 王耀发 创建
    /// 2016-6-24 杨浩 新增海关总署的配置
    /// </remarks>
    [Serializable]
    public class CustomsConfig : ConfigBase
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string EntRecordName { get; set; }
        /// <summary>
        /// 广州海关报文采用标准的AES加密，加密的密钥 Key
        /// </summary>
        [XmlElement(IsNullable=true)]
        public string Key { get; set; }
        /// <summary>
        ///报文类型
        ///</summary>
        [XmlElement(IsNullable = true)]
        public string MessageType { get; set; }

        /// <summary>
        /// 企业备案号
        /// </summary>
       [XmlElement(IsNullable = true)]
        public string SenderID { get; set; }

        /// <summary>
        /// 海关FTP地址
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string FtpUrl { get; set; }

        /// <summary>
        /// 海关FTP地址用户名
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string FtpUserName { get; set; }

        /// <summary>
        /// 海关FTP地址密码
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string FtpPassword { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string Version { get; set; }

        #region 海关总署单一平台
        /// <summary>
        /// 海关十位编码
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string EbcCode { get; set; }
        /// <summary>
        /// 海关注册登记企业名称
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string EbcName { get; set; }
        /// <summary>
        /// 海关总署单一平台FTP账号
        /// </summary>
        public string EbcFTPName { get; set; }
        /// <summary>
        /// 海关总署单一平台FTP地址
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string EbcFtpUrl { get; set; }
        /// <summary>
        /// 海关总署单一平台FTP密码
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string EbcFtpPassword { get; set; }
        /// <summary>
        /// 单一窗口DxpId编码
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string DxpId { get; set; }

        #endregion

    }
}
