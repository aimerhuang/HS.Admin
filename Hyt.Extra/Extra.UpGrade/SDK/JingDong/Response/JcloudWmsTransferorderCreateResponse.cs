using System;
using System.Xml.Serialization;
using System.Collections.Generic;

										namespace Extra.UpGrade.SDK.JingDong.Response
{





public class JcloudWmsTransferorderCreateResponse : JdResponse{


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
