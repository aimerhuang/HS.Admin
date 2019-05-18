using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderResponse : JdObject{


         [XmlElement("process_code")]
public  		string
  processCode { get; set; }


         [XmlElement("process_status")]
public  		string
  processStatus { get; set; }


         [XmlElement("error_message")]
public  		string
  errorMessage { get; set; }


         [XmlElement("total_page")]
public  		string
  totalPage { get; set; }


         [XmlElement("cur_page_num")]
public  		string
  curPageNum { get; set; }


         [XmlElement("order_list")]
public  		List<string>
  orderList { get; set; }


}
}
