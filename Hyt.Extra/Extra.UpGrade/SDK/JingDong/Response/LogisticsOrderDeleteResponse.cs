using System;
using System.Xml.Serialization;
using System.Collections.Generic;

										namespace Extra.UpGrade.SDK.JingDong.Response
{





public class LogisticsOrderDeleteResponse : JdResponse{


         [XmlElement("process_code")]
public  		string
  processCode { get; set; }


         [XmlElement("process_status")]
public  		string
  processStatus { get; set; }


         [XmlElement("error_message")]
public  		string
  errorMessage { get; set; }


}
}
