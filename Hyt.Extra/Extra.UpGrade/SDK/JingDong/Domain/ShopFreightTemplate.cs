using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ShopFreightTemplate : JdObject{


         [XmlElement("id")]
public  		string
  id { get; set; }


         [XmlElement("templateName")]
public  		string
  templateName { get; set; }


}
}
