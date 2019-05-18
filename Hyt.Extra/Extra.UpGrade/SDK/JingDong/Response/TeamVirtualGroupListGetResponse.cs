using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class TeamVirtualGroupListGetResponse : JdResponse{


         [XmlElement("virtual_group_list_for_jos")]
public  		string
  virtualGroupListForJos { get; set; }


}
}
