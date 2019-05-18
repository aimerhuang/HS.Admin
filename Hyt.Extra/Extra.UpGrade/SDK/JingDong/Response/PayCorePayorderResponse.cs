using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class PayCorePayorderResponse : JdResponse{


         [XmlElement("payorderextsign_result")]
public  		string
  payorderextsignResult { get; set; }


}
}
