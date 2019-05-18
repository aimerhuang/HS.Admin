using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class RefundapplyOperatorResponse : JdObject{


         [XmlElement("result_state")]
public  		int
  resultState { get; set; }


         [XmlElement("result_info")]
public  		string
  resultInfo { get; set; }


}
}
