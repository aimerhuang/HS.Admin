using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ADQuery : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("skuId")]
public  		string
  skuId { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("auditInfoList")]
public  		List<string>
  auditInfoList { get; set; }


         [XmlElement("imgUrl")]
public  		string
  imgUrl { get; set; }


}
}
