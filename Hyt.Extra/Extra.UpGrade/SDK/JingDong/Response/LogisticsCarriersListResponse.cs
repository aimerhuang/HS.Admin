using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class LogisticsCarriersListResponse : JdResponse{


         [XmlElement("carriers_details")]
public  		string
  carriersDetails { get; set; }


}
}
