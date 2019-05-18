using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SkuListByTeamIdForJos : JdObject{


         [XmlElement("sku_list")]
public  		List<string>
  skuList { get; set; }


         [XmlElement("result_code")]
public  		string
  resultCode { get; set; }


}
}
