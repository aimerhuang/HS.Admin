using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class TeamNearbycountGetResponse : JdResponse{


         [XmlElement("nearby_team_count_for_jos")]
public  		string
  nearbyTeamCountForJos { get; set; }


}
}
