using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class TempCompletePage : JdObject{


         [XmlElement("totalNum")]
public  		string
  totalNum { get; set; }


         [XmlElement("tempCompleteList")]
public  		List<string>
  tempCompleteList { get; set; }


}
}
