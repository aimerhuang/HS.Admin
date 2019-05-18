using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ConsigneeClient : JdObject{


         [XmlElement("addressLevel1Id")]
public  		string
  addressLevel1Id { get; set; }


         [XmlElement("addressLevel2Id")]
public  		string
  addressLevel2Id { get; set; }


         [XmlElement("addressLevel3Id")]
public  		string
  addressLevel3Id { get; set; }


         [XmlElement("addressLevel4Id")]
public  		string
  addressLevel4Id { get; set; }


         [XmlElement("addressDetail")]
public  		string
  addressDetail { get; set; }


         [XmlElement("phone")]
public  		string
  phone { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("idCard")]
public  		string
  idCard { get; set; }


}
}
