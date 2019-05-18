using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WareSkuPictureApiVO : JdObject{


         [XmlElement("wareId")]
public  		long
  wareId { get; set; }


         [XmlElement("attrValueId")]
public  		int
  attrValueId { get; set; }


         [XmlElement("imgUrl")]
public  		string
  imgUrl { get; set; }


         [XmlElement("indexId")]
public  		int
  indexId { get; set; }


}
}
