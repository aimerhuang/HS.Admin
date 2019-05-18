using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PoBoxModel : JdObject{


         [XmlElement("boxNo")]
public  		string
  boxNo { get; set; }


         [XmlElement("goodsNo")]
public  		string
  goodsNo { get; set; }


         [XmlElement("realInstoreQty")]
public  		string
  realInstoreQty { get; set; }


}
}
