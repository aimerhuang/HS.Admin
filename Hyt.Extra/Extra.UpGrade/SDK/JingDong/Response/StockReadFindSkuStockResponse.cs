using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class StockReadFindSkuStockResponse : JdResponse{


         [XmlElement("skuStocks")]
public  		List<string>
  skuStocks { get; set; }


}
}
