using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosPage : JdObject{


         [XmlElement("index")]
public  		string
  index { get; set; }


         [XmlElement("size")]
public  		string
  size { get; set; }


         [XmlElement("total")]
public  		string
  total { get; set; }


         [XmlElement("err_cod")]
public  		string
  errCod { get; set; }


         [XmlElement("err_msg")]
public  		string
  errMsg { get; set; }


         [XmlElement("content")]
public  		List<string>
  content { get; set; }


}
}
