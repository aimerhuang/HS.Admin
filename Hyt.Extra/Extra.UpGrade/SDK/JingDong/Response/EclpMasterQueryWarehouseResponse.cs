using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EclpMasterQueryWarehouseResponse : JdResponse{


         [XmlElement("querywarehouse_result")]
public  		List<string>
  querywarehouseResult { get; set; }


}
}
