using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EptOrderDeliveryorderResponse : JdResponse{


         [XmlElement("deliveryorder_result")]
public  		string
  deliveryorderResult { get; set; }


}
}
