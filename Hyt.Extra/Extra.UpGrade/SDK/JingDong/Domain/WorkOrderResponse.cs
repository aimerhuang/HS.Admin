using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WorkOrderResponse : JdObject{


         [XmlElement("result_code")]
public  		string
  resultCode { get; set; }


         [XmlElement("result_msg")]
public  		string
  resultMsg { get; set; }


}
}
