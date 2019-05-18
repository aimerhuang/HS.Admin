using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ArrayList : JdObject{


         [XmlElement("crowdId")]
public  		long
  crowdId { get; set; }


}
}
