using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ObjA_Price : JdObject{


         [XmlElement("max")]
public  		string
  max { get; set; }


         [XmlElement("min")]
public  		string
  min { get; set; }


}
}
