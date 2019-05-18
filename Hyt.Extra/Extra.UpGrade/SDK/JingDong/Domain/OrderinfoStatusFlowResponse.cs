using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderinfoStatusFlowResponse : JdObject{


         [XmlElement("state_value")]
public  		string
  stateValue { get; set; }


         [XmlElement("state_time")]
public  		string
  stateTime { get; set; }


         [XmlElement("state_operator")]
public  		string
  stateOperator { get; set; }


         [XmlElement("state_remark")]
public  		string
  stateRemark { get; set; }


}
}
