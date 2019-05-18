using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WarePropimgsGetResponse : JdResponse{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("propimgs")]
public  		List<string>
  propimgs { get; set; }


         [XmlElement("total")]
public  		string
  total { get; set; }


}
}
