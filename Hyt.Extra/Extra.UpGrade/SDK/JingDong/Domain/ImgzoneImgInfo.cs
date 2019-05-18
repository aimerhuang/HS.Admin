using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ImgzoneImgInfo : JdObject{


         [XmlElement("picture_id")]
public  		string
  pictureId { get; set; }


         [XmlElement("picture_cate_id")]
public  		int
  pictureCateId { get; set; }


         [XmlElement("picture_url")]
public  		string
  pictureUrl { get; set; }


         [XmlElement("picture_name")]
public  		string
  pictureName { get; set; }


         [XmlElement("picture_type")]
public  		string
  pictureType { get; set; }


         [XmlElement("referenced")]
public  		int
  referenced { get; set; }


         [XmlElement("picture_size")]
public  		string
  pictureSize { get; set; }


         [XmlElement("picture_width")]
public  		string
  pictureWidth { get; set; }


         [XmlElement("picture_height")]
public  		string
  pictureHeight { get; set; }


         [XmlElement("created")]
public  		DateTime
  created { get; set; }


         [XmlElement("modified")]
public  		DateTime
  modified { get; set; }


}
}
