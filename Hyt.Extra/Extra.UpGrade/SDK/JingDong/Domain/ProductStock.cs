using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ProductStock : JdObject{


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("flag")]
public  		bool
  flag { get; set; }


         [XmlElement("stockStatus")]
public  		string
  stockStatus { get; set; }


}
}
