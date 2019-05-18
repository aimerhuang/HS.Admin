using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class TeamGoodsCategoryListGetResponse : JdResponse{


         [XmlElement("goods_category_list_for_jos")]
public  		string
  goodsCategoryListForJos { get; set; }


}
}
