using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class VenderChildAccountQueryResponse : JdResponse{


         [XmlElement("child_account_result")]
public  		string
  childAccountResult { get; set; }


}
}
