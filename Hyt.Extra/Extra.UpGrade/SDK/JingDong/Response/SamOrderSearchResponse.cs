using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class SamOrderSearchResponse : JdResponse{


         [XmlElement("returnfinishedorderinfo_result")]
public  		string
  returnfinishedorderinfoResult { get; set; }


}
}
