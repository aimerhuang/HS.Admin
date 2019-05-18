using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderInfoOperateResponse : JdObject{


         [XmlElement("stateCode")]
public  		int
  stateCode { get; set; }


         [XmlElement("stateMessage")]
public  		string
  stateMessage { get; set; }


}
}
