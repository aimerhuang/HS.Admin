using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class LdopReceiveTraceGetResponse : JdResponse{


         [XmlElement("querytrace_result")]
public  		string
  querytraceResult { get; set; }


}
}
