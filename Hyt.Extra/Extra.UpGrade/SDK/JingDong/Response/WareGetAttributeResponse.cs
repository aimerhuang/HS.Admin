using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WareGetAttributeResponse : JdResponse{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("attributes")]
public  		List<string>
  attributes { get; set; }


         [XmlElement("total")]
public  		string
  total { get; set; }


}
}
