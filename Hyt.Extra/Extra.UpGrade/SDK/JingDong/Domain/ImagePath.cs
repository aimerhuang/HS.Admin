using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ImagePath : JdObject{


         [XmlElement("sku_id")]
public  		long
  skuId { get; set; }


         [XmlElement("image_list")]
public  		List<string>
  imageList { get; set; }


}
}
