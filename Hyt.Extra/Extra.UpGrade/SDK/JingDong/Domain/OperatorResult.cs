using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OperatorResult : JdObject{


         [XmlElement("logiNo")]
public  		string
  logiNo { get; set; }


         [XmlElement("chineseErrCode")]
public  		string
  chineseErrCode { get; set; }


         [XmlElement("englishErrCode")]
public  		string
  englishErrCode { get; set; }


         [XmlElement("errorCode")]
public  		string
  errorCode { get; set; }


         [XmlElement("success")]
public  		bool
  success { get; set; }


         [XmlElement("outBatchId")]
public  		long
  outBatchId { get; set; }


         [XmlElement("sendbatchId")]
public  		long
  sendbatchId { get; set; }


}
}
