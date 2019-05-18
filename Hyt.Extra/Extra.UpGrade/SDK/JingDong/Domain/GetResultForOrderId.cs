using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class GetResultForOrderId : JdObject{


         [XmlElement("success")]
public  		bool
  success { get; set; }


         [XmlElement("errorMsg")]
public  		string
  errorMsg { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("idCardNumber")]
public  		string
  idCardNumber { get; set; }


         [XmlElement("frontImgUrl")]
public  		string
  frontImgUrl { get; set; }


         [XmlElement("backImgUrl")]
public  		string
  backImgUrl { get; set; }


}
}
