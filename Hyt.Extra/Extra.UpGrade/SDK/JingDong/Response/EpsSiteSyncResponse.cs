using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EpsSiteSyncResponse : JdResponse{


         [XmlElement("sitesync_result")]
public  		string
  sitesyncResult { get; set; }


}
}
