using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class ServicePromotionPickedGoodsResponse : JdResponse{


         [XmlElement("getpickedgoods_result")]
public  		string
  getpickedgoodsResult { get; set; }


}
}
