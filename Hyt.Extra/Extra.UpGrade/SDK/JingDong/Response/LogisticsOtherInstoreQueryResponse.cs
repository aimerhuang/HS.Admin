using System;
using System.Xml.Serialization;
using System.Collections.Generic;

																								using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class LogisticsOtherInstoreQueryResponse : JdResponse{


         [XmlElement("process_code")]
public  		string
  processCode { get; set; }


         [XmlElement("process_status")]
public  		string
  processStatus { get; set; }


         [XmlElement("error_message")]
public  		string
  errorMessage { get; set; }


         [XmlElement("inbound_no")]
public  		string
  inboundNo { get; set; }


         [XmlElement("po_no")]
public  		string
  poNo { get; set; }


         [XmlElement("inbound_status")]
public  		string
  inboundStatus { get; set; }


         [XmlElement("status_update_time")]
public  		DateTime
  statusUpdateTime { get; set; }


         [XmlElement("task_details")]
public  		List<string>
  taskDetails { get; set; }


}
}
