using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WareDelistingGetResponse : JdResponse{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("ware_infos")]
public  		List<string>
  wareInfos { get; set; }


         [XmlElement("total")]
public  		string
  total { get; set; }


}
}
