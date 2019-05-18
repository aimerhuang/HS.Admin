using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class GradePromotion : JdObject{


         [XmlElement("cur_grade")]
public  		string
  curGrade { get; set; }


         [XmlElement("cur_grade_name")]
public  		string
  curGradeName { get; set; }


         [XmlElement("next_upgrade_amount")]
public  		string
  nextUpgradeAmount { get; set; }


         [XmlElement("next_upgrade_count")]
public  		string
  nextUpgradeCount { get; set; }


         [XmlElement("next_grade_name")]
public  		string
  nextGradeName { get; set; }


         [XmlElement("next_grade")]
public  		string
  nextGrade { get; set; }


}
}
