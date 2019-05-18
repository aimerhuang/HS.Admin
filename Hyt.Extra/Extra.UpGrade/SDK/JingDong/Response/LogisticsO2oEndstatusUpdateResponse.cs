using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class LogisticsO2oEndstatusUpdateResponse : JdResponse{


         [XmlElement("OrderResponseStatus")]
public  		string
  OrderResponseStatus { get; set; }


}
}
