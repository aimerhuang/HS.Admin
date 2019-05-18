using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class AreaIpQueryResponse : JdResponse{


         [XmlElement("jip_response")]
public  		string
  jipResponse { get; set; }


}
}
