using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ErpUserDto : JdObject{


         [XmlElement("erp_code")]
public  		string
  erpCode { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


}
}
