using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class IsvCheckStockDetail : JdObject{


         [XmlElement("goodsNo")]
public  		string
  goodsNo { get; set; }


         [XmlElement("goodsName")]
public  		string
  goodsName { get; set; }


         [XmlElement("diffQty")]
public  		string
  diffQty { get; set; }


         [XmlElement("oneLevelReason")]
public  		string
  oneLevelReason { get; set; }


         [XmlElement("twoLevelReason")]
public  		string
  twoLevelReason { get; set; }


         [XmlElement("threeLevelReason")]
public  		string
  threeLevelReason { get; set; }


}
}
