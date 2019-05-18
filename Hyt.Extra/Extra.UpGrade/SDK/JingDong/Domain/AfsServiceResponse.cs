using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AfsServiceResponse : JdObject{


         [XmlElement("process_code")]
public  		int
  processCode { get; set; }


         [XmlElement("process_status")]
public  		string
  processStatus { get; set; }


         [XmlElement("error_message")]
public  		string
  errorMessage { get; set; }


         [XmlElement("total_page")]
public  		int
  totalPage { get; set; }


         [XmlElement("cur_page_num")]
public  		int
  curPageNum { get; set; }


         [XmlElement("customer_order_list")]
public  		List<string>
  customerOrderList { get; set; }


}
}
