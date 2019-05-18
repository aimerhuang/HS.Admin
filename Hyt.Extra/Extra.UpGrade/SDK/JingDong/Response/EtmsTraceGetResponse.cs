using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EtmsTraceGetResponse : JdResponse{


         [XmlElement("trace_api_dtos")]
public  		List<string>
  traceApiDtos { get; set; }


}
}
