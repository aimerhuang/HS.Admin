using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class SamOrderInfoQueryResponse : JdResponse{


         [XmlElement("queryorderinfo_result")]
public  		string
  queryorderinfoResult { get; set; }


}
}
