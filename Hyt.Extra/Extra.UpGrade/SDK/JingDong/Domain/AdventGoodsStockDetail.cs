using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AdventGoodsStockDetail : JdObject{


         [XmlElement("goodsNo")]
public  		string
  goodsNo { get; set; }


         [XmlElement("goodsName")]
public  		string
  goodsName { get; set; }


         [XmlElement("stockStatus")]
public  		string
  stockStatus { get; set; }


         [XmlElement("num")]
public  		string
  num { get; set; }


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
