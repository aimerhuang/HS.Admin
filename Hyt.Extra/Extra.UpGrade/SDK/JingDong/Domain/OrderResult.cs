using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderResult : JdObject{


         [XmlElement("apiResult")]
public  		string
  apiResult { get; set; }


         [XmlElement("orderInfo")]
public  		string
  orderInfo { get; set; }


}
}
