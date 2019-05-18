using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PageList : JdObject{


         [XmlElement("datas")]
public  		List<string>
  datas { get; set; }


         [XmlElement("paginator")]
public  		string
  paginator { get; set; }


}
}
