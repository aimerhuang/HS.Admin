using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VcInStockSkuDto : JdObject{


         [XmlElement("goodsSku")]
public  		string
  goodsSku { get; set; }


         [XmlElement("goodsName")]
public  		string
  goodsName { get; set; }


         [XmlElement("goodsCount")]
public  		string
  goodsCount { get; set; }


         [XmlElement("companyCode")]
public  		string
  companyCode { get; set; }


         [XmlElement("distribCenterCode")]
public  		string
  distribCenterCode { get; set; }


         [XmlElement("warehouseCode")]
public  		string
  warehouseCode { get; set; }


}
}
