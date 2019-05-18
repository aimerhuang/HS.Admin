using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class RequisitionInfoResultDto : JdObject{


         [XmlElement("requisition_info_detail")]
public  		string
  requisitionInfoDetail { get; set; }


         [XmlElement("success")]
public  		string
  success { get; set; }


         [XmlElement("result_code")]
public  		string
  resultCode { get; set; }


         [XmlElement("result_message")]
public  		string
  resultMessage { get; set; }


}
}
