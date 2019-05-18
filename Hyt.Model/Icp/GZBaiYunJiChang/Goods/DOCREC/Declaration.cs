using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Model.Icp.GZBaiYunJiChang.Goods.DOCREC
{
    public class Declaration
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "OrgMessageID")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string OrgMessageID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "OrgMessageType")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string OrgMessageType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "OrgSenderID")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string OrgSenderID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "OrgReceiverID")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string OrgReceiverID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "OrgRecTime")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string OrgRecTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "Status")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [XmlElement(ElementName = "Notes")]
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string Notes { get; set; }
    }
}
