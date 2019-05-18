using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VcUserPurchaserDto : JdObject{


         [XmlElement("erp_code")]
public  		string
  erpCode { get; set; }


         [XmlElement("erp_name")]
public  		string
  erpName { get; set; }


}
}
