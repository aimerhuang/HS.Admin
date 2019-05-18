using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderPrintDataConsignee : JdObject{


         [XmlElement("cons_name")]
public  		string
  consName { get; set; }


         [XmlElement("cons_address")]
public  		string
  consAddress { get; set; }


         [XmlElement("cons_phone")]
public  		string
  consPhone { get; set; }


         [XmlElement("cons_handset")]
public  		string
  consHandset { get; set; }


}
}
