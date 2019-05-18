using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AreaListForJos : JdObject{


         [XmlElement("team_area_list")]
public  		List<string>
  teamAreaList { get; set; }


         [XmlElement("result_code")]
public  		string
  resultCode { get; set; }


}
}
