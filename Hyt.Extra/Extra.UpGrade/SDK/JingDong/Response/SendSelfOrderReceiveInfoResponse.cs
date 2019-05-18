using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class SendSelfOrderReceiveInfoResponse : JdResponse{


         [XmlElement("sendselforderreceiveinfo_result")]
public  		string
  sendselforderreceiveinfoResult { get; set; }


}
}
