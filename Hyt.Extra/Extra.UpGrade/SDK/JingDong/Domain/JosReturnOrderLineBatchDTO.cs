using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosReturnOrderLineBatchDTO : JdObject{


         [XmlElement("returnOrderCode")]
public  		string
  returnOrderCode { get; set; }


         [XmlElement("jdSku")]
public  		string
  jdSku { get; set; }


         [XmlElement("returnPrice")]
public  		string
  returnPrice { get; set; }


         [XmlElement("returnQuantity")]
public  		string
  returnQuantity { get; set; }


         [XmlElement("purchasePrice")]
public  		string
  purchasePrice { get; set; }


         [XmlElement("purchaseOrderCode")]
public  		string
  purchaseOrderCode { get; set; }


         [XmlElement("purchaseDate")]
public  		DateTime
  purchaseDate { get; set; }


         [XmlElement("comments")]
public  		string
  comments { get; set; }


         [XmlElement("createTime")]
public  		DateTime
  createTime { get; set; }


         [XmlElement("updateTime")]
public  		DateTime
  updateTime { get; set; }


}
}
