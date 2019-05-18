using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class GoodsCategoryListForJos : JdObject{


         [XmlElement("goods_category_list")]
public  		List<string>
  goodsCategoryList { get; set; }


         [XmlElement("result_code")]
public  		string
  resultCode { get; set; }


}
}
