using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AllAppCustomers : JdObject{


         [XmlElement("totalResult")]
public  		string
  totalResult { get; set; }


         [XmlElement("appCustomers")]
public  		List<string>
  appCustomers { get; set; }


}
}
