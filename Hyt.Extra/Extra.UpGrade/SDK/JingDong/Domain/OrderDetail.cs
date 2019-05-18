using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderDetail : JdObject{


         [XmlElement("goodsNo")]
public  		string
  goodsNo { get; set; }


         [XmlElement("price")]
public  		double
  price { get; set; }


         [XmlElement("quantity")]
public  		int
  quantity { get; set; }


         [XmlElement("shopGoodsNo")]
public  		string
  shopGoodsNo { get; set; }


         [XmlElement("isvGoodsNo")]
public  		string
  isvGoodsNo { get; set; }


}
}
