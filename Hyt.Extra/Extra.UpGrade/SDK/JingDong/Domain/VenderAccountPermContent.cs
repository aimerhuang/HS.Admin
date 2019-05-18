using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VenderAccountPermContent : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("level")]
public  		int
  level { get; set; }


         [XmlElement("order_num")]
public  		int
  orderNum { get; set; }


         [XmlElement("parent_id")]
public  		long
  parentId { get; set; }


}
}
