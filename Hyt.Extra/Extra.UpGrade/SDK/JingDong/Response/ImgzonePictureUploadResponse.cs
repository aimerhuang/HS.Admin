using System;
using System.Xml.Serialization;
using System.Collections.Generic;

													namespace Extra.UpGrade.SDK.JingDong.Response
{





public class ImgzonePictureUploadResponse : JdResponse{


         [XmlElement("return_code")]
public  		string
  returnCode { get; set; }


         [XmlElement("desc")]
public  		string
  desc { get; set; }


         [XmlElement("picture_id")]
public  		string
  pictureId { get; set; }


         [XmlElement("picture_url")]
public  		string
  pictureUrl { get; set; }


}
}
