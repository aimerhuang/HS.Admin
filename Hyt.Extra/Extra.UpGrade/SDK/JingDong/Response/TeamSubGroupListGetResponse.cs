using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class TeamSubGroupListGetResponse : JdResponse{


         [XmlElement("sub_group_list_for_jos")]
public  		string
  subGroupListForJos { get; set; }


}
}
