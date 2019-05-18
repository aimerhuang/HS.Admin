using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class UnionOrderServiceQueryOrdersResponse : JdResponse{


         [XmlElement("queryorders_result")]
public  		string
  queryordersResult { get; set; }


}
}
