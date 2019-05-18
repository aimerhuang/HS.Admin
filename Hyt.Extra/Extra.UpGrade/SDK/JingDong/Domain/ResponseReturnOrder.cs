using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ResponseReturnOrder : JdObject{


         [XmlElement("process_code")]
public  		string
  processCode { get; set; }


         [XmlElement("process_status")]
public  		string
  processStatus { get; set; }


         [XmlElement("error_message")]
public  		string
  errorMessage { get; set; }


         [XmlElement("josl_inbound_no")]
public  		string
  joslInboundNo { get; set; }


         [XmlElement("return_order_no")]
public  		string
  returnOrderNo { get; set; }


         [XmlElement("josl_outbound_no")]
public  		string
  joslOutboundNo { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("complete_time")]
public  		string
  completeTime { get; set; }


}
}
