using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EclpCloudReceiveOrderInfoResponse : JdResponse{


         [XmlElement("receiveorderinfo_result")]
public  		string
  receiveorderinfoResult { get; set; }


}
}
