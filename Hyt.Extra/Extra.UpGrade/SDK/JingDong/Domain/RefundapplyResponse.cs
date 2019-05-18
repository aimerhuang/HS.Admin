using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class RefundapplyResponse : JdObject{


         [XmlElement("results")]
public  		List<string>
  results { get; set; }


         [XmlElement("result_state")]
public  		bool
  resultState { get; set; }


         [XmlElement("result_info")]
public  		string
  resultInfo { get; set; }


}
}
