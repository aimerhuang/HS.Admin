using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class GoodsSIDResponse : JdObject{


         [XmlElement("goodsSIDList")]
public  		List<string>
  goodsSIDList { get; set; }


}
}
