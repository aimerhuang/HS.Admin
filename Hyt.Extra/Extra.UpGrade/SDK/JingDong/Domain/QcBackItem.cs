using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QcBackItem : JdObject{


         [XmlElement("goodsName")]
public  		string
  goodsName { get; set; }


         [XmlElement("goodsNo")]
public  		string
  goodsNo { get; set; }


         [XmlElement("sellerGoodsSign")]
public  		string
  sellerGoodsSign { get; set; }


         [XmlElement("serialNo")]
public  		string
  serialNo { get; set; }


         [XmlElement("qualifiedQty")]
public  		string
  qualifiedQty { get; set; }


         [XmlElement("checkResultStr")]
public  		string
  checkResultStr { get; set; }


         [XmlElement("qcTimeStr")]
public  		string
  qcTimeStr { get; set; }


}
}
