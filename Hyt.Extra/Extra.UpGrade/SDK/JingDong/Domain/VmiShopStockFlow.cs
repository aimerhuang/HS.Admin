using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VmiShopStockFlow : JdObject{


         [XmlElement("flowId")]
public  		string
  flowId { get; set; }


         [XmlElement("createTime")]
public  		string
  createTime { get; set; }


         [XmlElement("deptNo")]
public  		string
  deptNo { get; set; }


         [XmlElement("warehouseNo")]
public  		string
  warehouseNo { get; set; }


         [XmlElement("goodsNo")]
public  		string
  goodsNo { get; set; }


         [XmlElement("spGoodsNo")]
public  		string
  spGoodsNo { get; set; }


         [XmlElement("formerNum")]
public  		string
  formerNum { get; set; }


         [XmlElement("nowNum")]
public  		string
  nowNum { get; set; }


         [XmlElement("bizTypeName")]
public  		string
  bizTypeName { get; set; }


}
}
