using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EclpPoExtQueryPoOrderResponse : JdResponse{


         [XmlElement("queryPoModelList")]
public  		List<string>
  queryPoModelList { get; set; }


}
}
