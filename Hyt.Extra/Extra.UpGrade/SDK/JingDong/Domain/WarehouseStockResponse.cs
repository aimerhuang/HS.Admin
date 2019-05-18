using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WarehouseStockResponse : JdObject{


         [XmlElement("deptNo")]
public  		string
  deptNo { get; set; }


         [XmlElement("deptName")]
public  		string
  deptName { get; set; }


         [XmlElement("warehouseNo")]
public  		string
  warehouseNo { get; set; }


         [XmlElement("warehouseName")]
public  		string
  warehouseName { get; set; }


         [XmlElement("goodsNo")]
public  		string
  goodsNo { get; set; }


         [XmlElement("goodsName")]
public  		string
  goodsName { get; set; }


         [XmlElement("sellerGoodsSign")]
public  		string
  sellerGoodsSign { get; set; }


         [XmlElement("stockStatus")]
public  		string
  stockStatus { get; set; }


         [XmlElement("stockType")]
public  		string
  stockType { get; set; }


         [XmlElement("totalNum")]
public  		string
  totalNum { get; set; }


         [XmlElement("usableNum")]
public  		string
  usableNum { get; set; }


         [XmlElement("ext1")]
public  		string
  ext1 { get; set; }


         [XmlElement("ext2")]
public  		string
  ext2 { get; set; }


         [XmlElement("ext3")]
public  		string
  ext3 { get; set; }


         [XmlElement("ext4")]
public  		string
  ext4 { get; set; }


         [XmlElement("ext5")]
public  		string
  ext5 { get; set; }


}
}
