using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class RoDetailDto : JdObject{


         [XmlElement("wareId")]
public  		long
  wareId { get; set; }


         [XmlElement("wareName")]
public  		string
  wareName { get; set; }


         [XmlElement("brandName")]
public  		string
  brandName { get; set; }


         [XmlElement("returnsPrice")]
public  		string
  returnsPrice { get; set; }


         [XmlElement("returnsNum")]
public  		int
  returnsNum { get; set; }


         [XmlElement("factNum")]
public  		int
  factNum { get; set; }


         [XmlElement("totalPrice")]
public  		string
  totalPrice { get; set; }


         [XmlElement("isbn")]
public  		string
  isbn { get; set; }


         [XmlElement("discount")]
public  		string
  discount { get; set; }


         [XmlElement("makePrice")]
public  		string
  makePrice { get; set; }


}
}
