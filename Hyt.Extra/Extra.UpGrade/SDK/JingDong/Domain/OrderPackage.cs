using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderPackage : JdObject{


         [XmlElement("packageNo")]
public  		string
  packageNo { get; set; }


         [XmlElement("packWeight")]
public  		double
  packWeight { get; set; }


}
}
