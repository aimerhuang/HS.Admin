using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class DspBalanceGetResponse : JdResponse{


         [XmlElement("getaccountbalance_result")]
public  		string
  getaccountbalanceResult { get; set; }


}
}
