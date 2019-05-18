using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class StoreMessage : JdObject{


         [XmlElement("returnCode")]
public  		string
  returnCode { get; set; }


         [XmlElement("result")]
public  		string
  result { get; set; }


         [XmlElement("data")]
public  		bool
  data { get; set; }


}
}
