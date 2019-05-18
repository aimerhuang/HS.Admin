using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WaitAuditApplysPage : JdObject{


         [XmlElement("applyInfoList")]
public  		List<string>
  applyInfoList { get; set; }


         [XmlElement("totalNum")]
public  		string
  totalNum { get; set; }


}
}
