using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VmiShopStockResponse : JdObject{


         [XmlElement("vmiShopStocks")]
public  		List<string>
  vmiShopStocks { get; set; }


}
}
