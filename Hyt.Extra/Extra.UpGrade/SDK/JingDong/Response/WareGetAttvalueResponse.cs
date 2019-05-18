using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WareGetAttvalueResponse : JdResponse{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("total")]
public  		string
  total { get; set; }


         [XmlElement("att_values")]
public  		List<string>
  attValues { get; set; }


}
}
