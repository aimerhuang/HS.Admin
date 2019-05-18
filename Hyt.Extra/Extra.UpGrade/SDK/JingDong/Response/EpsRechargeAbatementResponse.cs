using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EpsRechargeAbatementResponse : JdResponse{


         [XmlElement("rechargeabatement_result")]
public  		string
  rechargeabatementResult { get; set; }


}
}
