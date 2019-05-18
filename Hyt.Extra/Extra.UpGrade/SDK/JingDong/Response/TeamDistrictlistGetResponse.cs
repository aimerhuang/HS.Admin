using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class TeamDistrictlistGetResponse : JdResponse{


         [XmlElement("district_list_for_jos")]
public  		string
  districtListForJos { get; set; }


}
}
