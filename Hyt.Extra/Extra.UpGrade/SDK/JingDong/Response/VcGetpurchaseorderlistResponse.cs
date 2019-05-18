using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class VcGetpurchaseorderlistResponse : JdResponse{


         [XmlElement("jos_order_result_dto")]
public  		string
  josOrderResultDto { get; set; }


}
}
