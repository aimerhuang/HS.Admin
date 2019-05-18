using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VenderInfoResult : JdObject{


         [XmlElement("vender_id")]
public  		long
  venderId { get; set; }


         [XmlElement("col_type")]
public  		int
  colType { get; set; }


         [XmlElement("shop_id")]
public  		long
  shopId { get; set; }


         [XmlElement("shop_name")]
public  		string
  shopName { get; set; }


         [XmlElement("cate_main")]
public  		long
  cateMain { get; set; }


}
}
