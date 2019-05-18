using System;
using System.Xml.Serialization;
using System.Collections.Generic;

																			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class ImgzonePictureDeleteResponse : JdResponse{


         [XmlElement("return_code")]
public  		string
  returnCode { get; set; }


         [XmlElement("desc")]
public  		string
  desc { get; set; }


         [XmlElement("success_num")]
public  		string
  successNum { get; set; }


         [XmlElement("illegal")]
public  		string
  illegal { get; set; }


         [XmlElement("referenced")]
public  		string
  referenced { get; set; }


         [XmlElement("fail")]
public  		string
  fail { get; set; }


}
}
