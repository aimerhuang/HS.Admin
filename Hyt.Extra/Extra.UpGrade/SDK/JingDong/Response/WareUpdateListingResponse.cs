using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WareUpdateListingResponse : JdResponse{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("ware_id")]
public  		string
  wareId { get; set; }


         [XmlElement("modified")]
public  		string
  modified { get; set; }


}
}
