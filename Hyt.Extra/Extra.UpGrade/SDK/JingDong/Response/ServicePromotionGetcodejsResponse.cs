using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class ServicePromotionGetcodejsResponse : JdResponse{


         [XmlElement("queryjs_result")]
public  		string
  queryjsResult { get; set; }


}
}
