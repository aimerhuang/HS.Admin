using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EdiScGetResponse : JdResponse{


         [XmlElement("shipmentConfirmationResultDTO")]
public  		string
  shipmentConfirmationResultDTO { get; set; }


}
}
