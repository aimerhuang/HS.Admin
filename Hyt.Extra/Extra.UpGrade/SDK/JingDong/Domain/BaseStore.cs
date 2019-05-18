using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class BaseStore : JdObject{


         [XmlElement("storeCode")]
public  		string
  storeCode { get; set; }


         [XmlElement("storeName")]
public  		string
  storeName { get; set; }


}
}
