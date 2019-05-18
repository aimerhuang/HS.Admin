using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ImSelfOrderResultInfoDTO : JdObject{


         [XmlElement("isAuthorized")]
public  		string
  isAuthorized { get; set; }


         [XmlElement("imSelfOrderDealInfoDTOList")]
public  		List<string>
  imSelfOrderDealInfoDTOList { get; set; }


         [XmlElement("errorMessage")]
public  		string
  errorMessage { get; set; }


}
}
