using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class JwPurchaseOrderCancelOrderResponse : JdResponse{


         [XmlElement("cancelorder_result")]
public  		string
  cancelorderResult { get; set; }


}
}