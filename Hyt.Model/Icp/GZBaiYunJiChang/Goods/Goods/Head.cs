using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods
{
    /// <summary>
    /// 海关商检 报头
    /// </summary>
    public class Head
    {
        /// <summary>
        /// 报文编号 不可空
        /// </summary>
        /// 
        [XmlElement(ElementName = "MessageID")]
        /// <summary>
        /// 报文编号
        /// </summary>
        /// 
        public string MessageID { get; set; }
        /// <summary>
        /// 报文类型 固定 KJ881101
        /// </summary>
        /// 
        [XmlElement(ElementName = "MessageType")]
        public string MessageType { get; set; }
        /// <summary>
        /// 报文发送者标识  请填写ICIP企业备案号,不要填汉字
        /// </summary>
        [XmlElement(ElementName = "Sender")]
        public string Sender { get; set; }
        /// <summary>
        /// 报文接收人标识  固定为ICIP
        /// </summary>
        [XmlElement(ElementName = "Receiver")]
        public string Receiver { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        [XmlElement(ElementName = "SendTime")]
        public string SendTime { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        [XmlElement(ElementName = "FunctionCode")]
        public string FunctionCode { get; set; }
        /// <summary>
        /// 签名信息
        /// </summary>
        [XmlElement(ElementName = "SignerInfo")]
        public string SignerInfo { get; set; }
        /// <summary>
        /// 最大10字符，固定为：2.0 可空
        /// </summary>
        [XmlElement(ElementName = "Version")]
        public string Version { get; set; }
    }
}
