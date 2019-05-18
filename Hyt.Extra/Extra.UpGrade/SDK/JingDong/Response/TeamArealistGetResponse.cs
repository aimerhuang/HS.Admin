using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class TeamArealistGetResponse : JdResponse{


         [XmlElement("area_list_for_jos")]
public  		string
  areaListForJos { get; set; }


}
}
