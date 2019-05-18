using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class LogisticsWarehouseListResponse : JdResponse{


         [XmlElement("warehouse_details")]
public  		List<string>
  warehouseDetails { get; set; }


}
}
