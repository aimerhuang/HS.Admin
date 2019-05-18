using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class DspCampainStatusUpdateResponse : JdResponse{


         [XmlElement("updatestatus_result")]
public  		string
  updatestatusResult { get; set; }


}
}
