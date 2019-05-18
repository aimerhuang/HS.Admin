using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WareProductsortGetResponse : JdResponse{


         [XmlElement("product_sorts")]
public  		List<string>
  productSorts { get; set; }


}
}
