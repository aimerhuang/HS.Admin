using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WirelessWarePriceGetResponse : JdResponse{


         [XmlElement("price_changes")]
public  		List<string>
  priceChanges { get; set; }


}
}
