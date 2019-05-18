using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class MaterialLabelVO : JdObject{


         [XmlElement("id")]
public  		string
  id { get; set; }


         [XmlElement("label_buyoutPrice")]
public  		string
  labelBuyoutPrice { get; set; }


         [XmlElement("label_cpcPrice")]
public  		string
  labelCpcPrice { get; set; }


         [XmlElement("label_name")]
public  		string
  labelName { get; set; }


}
}
