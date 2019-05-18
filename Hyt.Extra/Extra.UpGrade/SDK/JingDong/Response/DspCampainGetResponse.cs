using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class DspCampainGetResponse : JdResponse{


         [XmlElement("findcampaignbyid_result")]
public  		string
  findcampaignbyidResult { get; set; }


}
}
