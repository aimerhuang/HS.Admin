using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class LogisticsReturnorderQueryResponse : JdResponse{


         [XmlElement("response_return_order")]
public  		string
  responseReturnOrder { get; set; }


}
}
