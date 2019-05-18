using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class StorageJsfResult : JdObject{


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("success")]
public  		string
  success { get; set; }


         [XmlElement("resultMsgChinese")]
public  		string
  resultMsgChinese { get; set; }


         [XmlElement("resultMsgEnglish")]
public  		string
  resultMsgEnglish { get; set; }


         [XmlElement("errorCode")]
public  		string
  errorCode { get; set; }


}
}
