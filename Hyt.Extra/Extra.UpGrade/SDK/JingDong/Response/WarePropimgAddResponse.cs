using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WarePropimgAddResponse : JdResponse{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("ware_id")]
public  		string
  wareId { get; set; }


         [XmlElement("attribute_value_id")]
public  		string
  attributeValueId { get; set; }


         [XmlElement("id")]
public  		string
  id { get; set; }


         [XmlElement("created")]
public  		string
  created { get; set; }


}
}
