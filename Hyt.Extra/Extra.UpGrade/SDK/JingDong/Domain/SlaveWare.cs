using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SlaveWare : JdObject{


         [XmlElement("SlaveContent")]
public  		string
  SlaveContent { get; set; }


         [XmlElement("Slavewareid")]
public  		string
  Slavewareid { get; set; }


}
}
