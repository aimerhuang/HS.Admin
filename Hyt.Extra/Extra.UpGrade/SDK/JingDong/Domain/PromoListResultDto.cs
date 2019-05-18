using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PromoListResultDto : JdObject{


         [XmlElement("success")]
public  		bool
  success { get; set; }


         [XmlElement("result_code")]
public  		int
  resultCode { get; set; }


         [XmlElement("result_message")]
public  		string
  resultMessage { get; set; }


         [XmlElement("record_count")]
public  		long
  recordCount { get; set; }


         [XmlElement("promo_list")]
public  		List<string>
  promoList { get; set; }


}
}
