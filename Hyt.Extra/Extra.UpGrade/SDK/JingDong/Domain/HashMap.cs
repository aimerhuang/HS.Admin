using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class HashMap : JdObject{


         [XmlElement("order_list")]
public  		List<string>
  orderList { get; set; }


}
}
