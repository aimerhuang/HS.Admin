using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class TourGroupListForJos : JdObject{


         [XmlElement("tour_group_list")]
public  		List<string>
  tourGroupList { get; set; }


         [XmlElement("result_code")]
public  		string
  resultCode { get; set; }


}
}
