using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VenderAccountRoleContent : JdObject{


         [XmlElement("account_name")]
public  		string
  accountName { get; set; }


         [XmlElement("role_id")]
public  		long
  roleId { get; set; }


         [XmlElement("role_name")]
public  		string
  roleName { get; set; }


}
}
