using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class List : JdObject{


         [XmlElement("itemNotPayInfo")]
public  		string
  itemNotPayInfo { get; set; }


}
}
