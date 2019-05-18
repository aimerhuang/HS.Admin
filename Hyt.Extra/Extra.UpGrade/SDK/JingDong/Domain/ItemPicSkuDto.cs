using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ItemPicSkuDto : JdObject{


         [XmlElement("image_path_dto_list")]
public  		List<string>
  imagePathDtoList { get; set; }


         [XmlElement("sku_id")]
public  		string
  skuId { get; set; }


}
}
