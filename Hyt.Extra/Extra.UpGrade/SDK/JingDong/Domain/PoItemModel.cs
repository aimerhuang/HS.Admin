using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PoItemModel : JdObject{


         [XmlElement("goodsNo")]
public  		string
  goodsNo { get; set; }


         [XmlElement("numApplication")]
public  		string
  numApplication { get; set; }


         [XmlElement("goodsStatus")]
public  		string
  goodsStatus { get; set; }


         [XmlElement("realInstoreQty")]
public  		string
  realInstoreQty { get; set; }


         [XmlElement("shortQty")]
public  		string
  shortQty { get; set; }


         [XmlElement("damagedQty")]
public  		string
  damagedQty { get; set; }


         [XmlElement("emptyQty")]
public  		string
  emptyQty { get; set; }


         [XmlElement("expiredQty")]
public  		string
  expiredQty { get; set; }


         [XmlElement("otherQty")]
public  		string
  otherQty { get; set; }


         [XmlElement("remark")]
public  		string
  remark { get; set; }


}
}
