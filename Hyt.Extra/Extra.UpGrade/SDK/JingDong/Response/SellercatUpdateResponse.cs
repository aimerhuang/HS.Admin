using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class SellercatUpdateResponse : JdResponse{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("modified")]
public  		string
  modified { get; set; }


         [XmlElement("cid")]
public  		string
  cid { get; set; }


}
}
