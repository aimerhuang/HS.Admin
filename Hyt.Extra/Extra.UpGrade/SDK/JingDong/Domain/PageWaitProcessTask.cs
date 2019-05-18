using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PageWaitProcessTask : JdObject{


         [XmlElement("totalCount")]
public  		string
  totalCount { get; set; }


         [XmlElement("result")]
public  		List<string>
  result { get; set; }


}
}
