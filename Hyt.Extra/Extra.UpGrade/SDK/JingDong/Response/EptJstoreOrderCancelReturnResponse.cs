using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EptJstoreOrderCancelReturnResponse : JdResponse{


         [XmlElement("storeMessage")]
public  		string
  storeMessage { get; set; }


}
}
