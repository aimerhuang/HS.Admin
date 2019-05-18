using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class OrderSearchResponse : JdResponse{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("order_search")]
public  		string
  orderSearch { get; set; }


}
}
