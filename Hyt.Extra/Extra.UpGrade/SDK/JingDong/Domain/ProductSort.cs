using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ProductSort : JdObject{


         [XmlElement("product_sort_id")]
public  		int
  productSortId { get; set; }


         [XmlElement("father_id")]
public  		int
  fatherId { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("is_delete")]
public  		int
  isDelete { get; set; }


         [XmlElement("grade")]
public  		int
  grade { get; set; }


         [XmlElement("conte")]
public  		string
  conte { get; set; }


         [XmlElement("sort")]
public  		int
  sort { get; set; }


         [XmlElement("is_fit_service")]
public  		int
  isFitService { get; set; }


}
}
