using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AttributeSetting : JdObject{


         [XmlElement("skuId")]
public  		long
  skuId { get; set; }


         [XmlElement("cid")]
public  		long
  cid { get; set; }


         [XmlElement("type")]
public  		int
  type { get; set; }


         [XmlElement("attId")]
public  		int
  attId { get; set; }


         [XmlElement("valueId")]
public  		int
  valueId { get; set; }


         [XmlElement("value")]
public  		string
  value { get; set; }


         [XmlElement("remark")]
public  		string
  remark { get; set; }


}
}
