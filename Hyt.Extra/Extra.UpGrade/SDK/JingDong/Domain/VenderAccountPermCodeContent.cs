using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VenderAccountPermCodeContent : JdObject{


         [XmlElement("account_name")]
public  		string
  accountName { get; set; }


         [XmlElement("codes")]
public  		string
  codes { get; set; }


}
}
