using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VirtualGroupListForJos : JdObject{


         [XmlElement("virtual_groupList")]
public  		List<string>
  virtualGroupList { get; set; }


         [XmlElement("result_code")]
public  		string
  resultCode { get; set; }


}
}
