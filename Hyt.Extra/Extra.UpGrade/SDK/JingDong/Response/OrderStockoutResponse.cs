using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class OrderStockoutResponse : JdResponse{


         [XmlElement("orderstockout_result")]
public  		string
  orderstockoutResult { get; set; }


}
}
