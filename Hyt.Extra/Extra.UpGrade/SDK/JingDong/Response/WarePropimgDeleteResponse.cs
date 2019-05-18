using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WarePropimgDeleteResponse : JdResponse{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("image_id")]
public  		string
  imageId { get; set; }


         [XmlElement("created")]
public  		string
  created { get; set; }


}
}
