using System;
using System.Xml.Serialization;
using System.Collections.Generic;

										namespace Extra.UpGrade.SDK.JingDong.Response
{





public class ImgzonePictureIsreferencedResponse : JdResponse{


         [XmlElement("return_code")]
public  		string
  returnCode { get; set; }


         [XmlElement("desc")]
public  		string
  desc { get; set; }


         [XmlElement("is_referenced")]
public  		string
  isReferenced { get; set; }


}
}
