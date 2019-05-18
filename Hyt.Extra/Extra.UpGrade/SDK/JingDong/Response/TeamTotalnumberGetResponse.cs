using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class TeamTotalnumberGetResponse : JdResponse{


         [XmlElement("current_team_count_for_jos")]
public  		string
  currentTeamCountForJos { get; set; }


}
}
