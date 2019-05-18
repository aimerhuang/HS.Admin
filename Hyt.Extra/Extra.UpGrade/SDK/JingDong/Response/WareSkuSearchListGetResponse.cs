using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WareSkuSearchListGetResponse : JdResponse{


         [XmlElement("productSkuList")]
public  		string
  productSkuList { get; set; }


}
}
