using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class UnitPromoDetailResultDto : JdObject{


         [XmlElement("ware_promotion")]
public  		string
  warePromotion { get; set; }


         [XmlElement("success")]
public  		bool
  success { get; set; }


         [XmlElement("result_code")]
public  		int
  resultCode { get; set; }


         [XmlElement("result_message")]
public  		string
  resultMessage { get; set; }


}
}
