using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WareBaseproductGetResponse : JdResponse{


         [XmlElement("product_base")]
public  		List<string>
  productBase { get; set; }


}
}
