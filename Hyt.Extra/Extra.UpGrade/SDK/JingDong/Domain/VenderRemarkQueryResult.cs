using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VenderRemarkQueryResult : JdObject{


         [XmlElement("api_jos_result")]
public  		string
  apiJosResult { get; set; }


         [XmlElement("vender_remark")]
public  		string
  venderRemark { get; set; }


}
}
