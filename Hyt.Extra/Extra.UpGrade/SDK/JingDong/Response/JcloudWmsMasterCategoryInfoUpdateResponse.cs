using System;
using System.Xml.Serialization;
using System.Collections.Generic;

										namespace Extra.UpGrade.SDK.JingDong.Response
{





public class JcloudWmsMasterCategoryInfoUpdateResponse : JdResponse{


         [XmlElement("resultCode")]
public  		string
  resultCode { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("rows")]
public  		int
  rows { get; set; }


}
}
