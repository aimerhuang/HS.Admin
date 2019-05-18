using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ComponentExport : JdObject{


         [XmlElement("codeS")]
public  		string
  codeS { get; set; }


         [XmlElement("nameS")]
public  		string
  nameS { get; set; }


}
}
