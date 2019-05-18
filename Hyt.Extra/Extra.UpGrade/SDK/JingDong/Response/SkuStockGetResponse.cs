using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class SkuStockGetResponse : JdResponse{


         [XmlElement("stockStatus")]
public  		string
  stockStatus { get; set; }


}
}
