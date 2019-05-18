using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VenderAccountContent : JdObject{


         [XmlElement("account_id")]
public  		long
  accountId { get; set; }


         [XmlElement("account_name")]
public  		string
  accountName { get; set; }


         [XmlElement("user_name")]
public  		string
  userName { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("is_phone_open")]
public  		string
  isPhoneOpen { get; set; }


         [XmlElement("phone")]
public  		string
  phone { get; set; }


         [XmlElement("auth_status")]
public  		string
  authStatus { get; set; }


}
}
