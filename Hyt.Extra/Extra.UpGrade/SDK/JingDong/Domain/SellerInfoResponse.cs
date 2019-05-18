using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SellerInfoResponse : JdObject{


         [XmlElement("sellerNo")]
public  		string
  sellerNo { get; set; }


         [XmlElement("shopWarehouseInfoList")]
public  		List<string>
  shopWarehouseInfoList { get; set; }


}
}
