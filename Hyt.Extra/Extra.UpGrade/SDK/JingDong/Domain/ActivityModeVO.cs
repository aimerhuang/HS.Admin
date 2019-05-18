using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ActivityModeVO : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("promo_id")]
public  		long
  promoId { get; set; }


         [XmlElement("num_bound")]
public  		int
  numBound { get; set; }


         [XmlElement("freq_bound")]
public  		int
  freqBound { get; set; }


         [XmlElement("per_max_num")]
public  		int
  perMaxNum { get; set; }


         [XmlElement("per_min_num")]
public  		int
  perMinNum { get; set; }


}
}
