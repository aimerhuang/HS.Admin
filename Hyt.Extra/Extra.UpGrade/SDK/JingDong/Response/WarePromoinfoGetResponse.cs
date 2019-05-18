using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WarePromoinfoGetResponse : JdResponse{


         [XmlElement("promoInfoList")]
public  		List<string>
  promoInfoList { get; set; }


}
}
