using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WorkOrderResult : JdObject{


         [XmlElement("success")]
public  		bool
  success { get; set; }


         [XmlElement("error_code")]
public  		string
  errorCode { get; set; }


         [XmlElement("error_msg")]
public  		string
  errorMsg { get; set; }


         [XmlElement("total_count")]
public  		int
  totalCount { get; set; }


         [XmlElement("workOrder_list")]
public  		List<string>
  workOrderList { get; set; }


}
}
