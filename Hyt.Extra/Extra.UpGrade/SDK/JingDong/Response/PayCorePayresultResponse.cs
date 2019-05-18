using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class PayCorePayresultResponse : JdResponse{


         [XmlElement("payresultextsign_result")]
public  		string
  payresultextsignResult { get; set; }


}
}
