using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VcWareHouseOutSpareCodeJosDto : JdObject{


         [XmlElement("returnPrice")]
public  		string
  returnPrice { get; set; }


         [XmlElement("wareSku")]
public  		string
  wareSku { get; set; }


         [XmlElement("wareName")]
public  		string
  wareName { get; set; }


         [XmlElement("remark")]
public  		string
  remark { get; set; }


         [XmlElement("snNo")]
public  		string
  snNo { get; set; }


         [XmlElement("spareCode")]
public  		string
  spareCode { get; set; }


}
}
