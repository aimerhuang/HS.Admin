using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class JwPurchaseOrderOrderStatusResponse : JdResponse{


         [XmlElement("orderstatus_result")]
public  		string
  orderstatusResult { get; set; }


}
}
