using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class DropshipDpsDeliveryResponse : JdResponse{


         [XmlElement("deliverResult")]
public  		string
  deliverResult { get; set; }


}
}
