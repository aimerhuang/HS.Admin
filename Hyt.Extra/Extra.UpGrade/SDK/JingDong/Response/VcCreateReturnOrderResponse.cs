using System;
using System.Xml.Serialization;
using System.Collections.Generic;

				namespace Extra.UpGrade.SDK.JingDong.Response
{





public class VcCreateReturnOrderResponse : JdResponse{


         [XmlElement("returnOrderIds")]
public  		List<string>
  returnOrderIds { get; set; }


}
}
