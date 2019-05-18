using System;
using System.Xml.Serialization;
using System.Collections.Generic;

									using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class JcloudWmsMachiningQueryResponse : JdResponse{


         [XmlElement("resultCode")]
public  		string
  resultCode { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("content")]
public  		string
  content { get; set; }


}
}
