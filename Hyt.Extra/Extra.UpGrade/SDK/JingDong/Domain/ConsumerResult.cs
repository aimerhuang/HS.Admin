using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ConsumerResult : JdObject{


         [XmlElement("result_code")]
public  		int
  resultCode { get; set; }


         [XmlElement("rsudoesult_message")]
public  		string
  rsudoesultMessage { get; set; }


         [XmlElement("is_success")]
public  		bool
  isSuccess { get; set; }


}
}
