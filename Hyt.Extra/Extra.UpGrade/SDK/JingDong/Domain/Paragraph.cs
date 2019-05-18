using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Paragraph : JdObject{


         [XmlElement("Content")]
public  		string
  Content { get; set; }


         [XmlElement("IcoTagInfo")]
public  		string
  IcoTagInfo { get; set; }


         [XmlElement("shop_id")]
public  		string
  shopId { get; set; }


         [XmlElement("wareid")]
public  		string
  wareid { get; set; }


         [XmlElement("cid1")]
public  		string
  cid1 { get; set; }


         [XmlElement("cid2")]
public  		string
  cid2 { get; set; }


         [XmlElement("catid")]
public  		string
  catid { get; set; }


         [XmlElement("good")]
public  		string
  good { get; set; }


         [XmlElement("cod")]
public  		string
  cod { get; set; }


         [XmlElement("ico")]
public  		string
  ico { get; set; }


         [XmlElement("SlaveParagraph")]
public  		List<string>
  SlaveParagraph { get; set; }


}
}
