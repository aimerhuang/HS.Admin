using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class TeamCountBykeywordGetResponse : JdResponse{


         [XmlElement("team_count_byKeyWord_for_jos")]
public  		string
  teamCountByKeyWordForJos { get; set; }


}
}
