using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SamClubJosMember : JdObject{


         [XmlElement("samCardNo")]
public  		string
  samCardNo { get; set; }


         [XmlElement("cardHolderNbr")]
public  		string
  cardHolderNbr { get; set; }


         [XmlElement("cardHolderType")]
public  		string
  cardHolderType { get; set; }


         [XmlElement("chTypeShortDesc")]
public  		string
  chTypeShortDesc { get; set; }


         [XmlElement("cardStatCd")]
public  		string
  cardStatCd { get; set; }


         [XmlElement("currStatusCode")]
public  		string
  currStatusCode { get; set; }


         [XmlElement("memberCode")]
public  		string
  memberCode { get; set; }


         [XmlElement("startDate")]
public  		DateTime
  startDate { get; set; }


         [XmlElement("expireDate")]
public  		DateTime
  expireDate { get; set; }


         [XmlElement("fullName")]
public  		string
  fullName { get; set; }


         [XmlElement("birthDate")]
public  		string
  birthDate { get; set; }


         [XmlElement("phoneNbr")]
public  		string
  phoneNbr { get; set; }


         [XmlElement("phoneNbr2")]
public  		string
  phoneNbr2 { get; set; }


         [XmlElement("emailAddress")]
public  		string
  emailAddress { get; set; }


         [XmlElement("certType")]
public  		int
  certType { get; set; }


         [XmlElement("ctzidNbr")]
public  		string
  ctzidNbr { get; set; }


         [XmlElement("driverNbr")]
public  		string
  driverNbr { get; set; }


         [XmlElement("passportNbr")]
public  		string
  passportNbr { get; set; }


         [XmlElement("crmCardNo")]
public  		string
  crmCardNo { get; set; }


         [XmlElement("crmPin")]
public  		string
  crmPin { get; set; }


}
}
