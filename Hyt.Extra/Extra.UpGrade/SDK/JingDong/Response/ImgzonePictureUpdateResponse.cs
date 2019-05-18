using System;
using System.Xml.Serialization;
using System.Collections.Generic;

							namespace Extra.UpGrade.SDK.JingDong.Response
{





public class ImgzonePictureUpdateResponse : JdResponse{


         [XmlElement("return_code")]
public  		string
  returnCode { get; set; }


         [XmlElement("desc")]
public  		string
  desc { get; set; }


}
}
