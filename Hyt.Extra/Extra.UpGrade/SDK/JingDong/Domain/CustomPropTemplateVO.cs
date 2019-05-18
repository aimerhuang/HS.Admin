using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CustomPropTemplateVO : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("title")]
public  		string
  title { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


}
}
