using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class MachiningDestDetail : JdObject{


         [XmlElement("destOwnerNo")]
public  		string
  destOwnerNo { get; set; }


         [XmlElement("destSkuNo")]
public  		string
  destSkuNo { get; set; }


         [XmlElement("destProductLevel")]
public  		string
  destProductLevel { get; set; }


         [XmlElement("destQty")]
public  		string
  destQty { get; set; }


}
}
