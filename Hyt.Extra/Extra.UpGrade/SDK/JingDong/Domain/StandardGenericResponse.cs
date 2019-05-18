using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class StandardGenericResponse : JdObject{


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("resultMsg")]
public  		string
  resultMsg { get; set; }


         [XmlElement("pageNo")]
public  		int
  pageNo { get; set; }


         [XmlElement("pageTotal")]
public  		int
  pageTotal { get; set; }


         [XmlElement("results")]
public  		List<string>
  results { get; set; }


}
}
