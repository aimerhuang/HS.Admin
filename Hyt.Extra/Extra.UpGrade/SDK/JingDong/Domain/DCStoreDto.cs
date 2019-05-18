using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class DCStoreDto : JdObject{


         [XmlElement("dcid")]
public  		int
  dcid { get; set; }


         [XmlElement("sid")]
public  		int
  sid { get; set; }


}
}
