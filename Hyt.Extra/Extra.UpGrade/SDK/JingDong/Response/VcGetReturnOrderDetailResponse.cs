using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class VcGetReturnOrderDetailResponse : JdResponse{


         [XmlElement("detailResultDto")]
public  		string
  detailResultDto { get; set; }


}
}
