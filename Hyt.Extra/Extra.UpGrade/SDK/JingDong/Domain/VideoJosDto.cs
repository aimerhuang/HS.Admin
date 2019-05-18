using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VideoJosDto : JdObject{


         [XmlElement("id")]
public  		string
  id { get; set; }


         [XmlElement("vendor_code")]
public  		string
  vendorCode { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("type")]
public  		string
  type { get; set; }


         [XmlElement("type_name")]
public  		string
  typeName { get; set; }


         [XmlElement("url")]
public  		string
  url { get; set; }


         [XmlElement("size")]
public  		string
  size { get; set; }


         [XmlElement("tag")]
public  		string
  tag { get; set; }


         [XmlElement("description")]
public  		string
  description { get; set; }


         [XmlElement("video_id")]
public  		string
  videoId { get; set; }


         [XmlElement("duration")]
public  		string
  duration { get; set; }


         [XmlElement("image_u_r_l")]
public  		string
  imageURL { get; set; }


         [XmlElement("video_status")]
public  		string
  videoStatus { get; set; }


         [XmlElement("video_status_name")]
public  		string
  videoStatusName { get; set; }


         [XmlElement("created_by")]
public  		string
  createdBy { get; set; }


         [XmlElement("created_time")]
public  		DateTime
  createdTime { get; set; }


         [XmlElement("modified_by")]
public  		string
  modifiedBy { get; set; }


         [XmlElement("modified_time")]
public  		DateTime
  modifiedTime { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("uploaded_time")]
public  		DateTime
  uploadedTime { get; set; }


         [XmlElement("completed_time")]
public  		DateTime
  completedTime { get; set; }


         [XmlElement("released_time")]
public  		DateTime
  releasedTime { get; set; }


         [XmlElement("video_unicode")]
public  		string
  videoUnicode { get; set; }


}
}
