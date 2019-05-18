using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class SellercatAddResponse : JdResponse{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("create_time")]
public  		string
  createTime { get; set; }


         [XmlElement("cid")]
public  		string
  cid { get; set; }


}
}
