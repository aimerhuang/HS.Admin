using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QcBackErrItem : JdObject{


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


         [XmlElement("unQualifiedQty")]
public  		string
  unQualifiedQty { get; set; }


         [XmlElement("checkResultStr")]
public  		string
  checkResultStr { get; set; }


         [XmlElement("errReason")]
public  		string
  errReason { get; set; }


         [XmlElement("qcTimeStr")]
public  		string
  qcTimeStr { get; set; }


}
}
