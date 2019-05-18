using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ShopCategory : JdObject{


         [XmlElement("cid")]
public  		string
  cid { get; set; }


         [XmlElement("parent_id")]
public  		string
  parentId { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("is_Parent")]
public  		bool
  isParent { get; set; }


         [XmlElement("is_Open")]
public  		bool
  isOpen { get; set; }


         [XmlElement("is_HomeShow")]
public  		bool
  isHomeShow { get; set; }


         [XmlElement("shop_id")]
public  		string
  shopId { get; set; }


         [XmlElement("index_id")]
public  		string
  indexId { get; set; }


}
}
