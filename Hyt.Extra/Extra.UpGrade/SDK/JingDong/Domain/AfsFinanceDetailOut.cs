using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





    [Serializable]
    public class AfsFinanceDetailOut : JdObject
    {


        [XmlElement("id")]
        public int
          id { get; set; }


        [XmlElement("orderId")]
        public long
          orderId { get; set; }


        [XmlElement("wareId")]
        public int
          wareId { get; set; }


        [XmlElement("financeId")]
        public int
          financeId { get; set; }


        [XmlElement("refundMoney")]
        public string
          refundMoney { get; set; }


        [XmlElement("payMoney")]
        public string
          payMoney { get; set; }


        [XmlElement("refundNum")]
        public string
          refundNum { get; set; }


        [XmlElement("account")]
        public string
          account { get; set; }


        [XmlElement("bank")]
        public string
          bank { get; set; }


        [XmlElement("createdDate")]
        public DateTime
          createdDate { get; set; }


    }
}
