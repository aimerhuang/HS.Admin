using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class AfterStateUpdateResponse : JdResponse{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("vender_id")]
public  		string
  venderId { get; set; }


         [XmlElement("modified")]
public  		string
  modified { get; set; }


         [XmlElement("return_id")]
public  		string
  returnId { get; set; }


}
}
