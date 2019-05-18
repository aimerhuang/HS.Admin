using System;
using System.Xml.Serialization;
using System.Collections.Generic;

						using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class VenderAnnouncementListResponse : JdResponse{


         [XmlElement("total_count")]
public  		int
  totalCount { get; set; }


         [XmlElement("announcement_v_o_s")]
public  		List<string>
  announcementVOS { get; set; }


}
}
