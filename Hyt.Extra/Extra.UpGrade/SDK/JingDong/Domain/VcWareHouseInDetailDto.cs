using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VcWareHouseInDetailDto : JdObject{


         [XmlElement("goodsSku")]
public  		string
  goodsSku { get; set; }


         [XmlElement("goodsName")]
public  		string
  goodsName { get; set; }


         [XmlElement("total")]
public  		int
  total { get; set; }


}
}
