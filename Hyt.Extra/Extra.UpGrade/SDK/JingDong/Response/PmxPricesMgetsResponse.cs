using System;
using System.Xml.Serialization;
using System.Collections.Generic;

				namespace Extra.UpGrade.SDK.JingDong.Response
{





public class PmxPricesMgetsResponse : JdResponse{


         [XmlElement("skuPriceList")]
public  		List<string>
  skuPriceList { get; set; }


}
}
