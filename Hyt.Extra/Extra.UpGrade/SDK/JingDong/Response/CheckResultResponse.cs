using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class CheckResultResponse : JdResponse{


         [XmlElement("checkresult_result")]
public  		string
  checkresultResult { get; set; }


}
}
