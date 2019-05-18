using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class KeywordVOQuery : JdObject{


         [XmlElement("keywordName")]
public  		string
  keywordName { get; set; }


         [XmlElement("searchCount")]
public  		string
  searchCount { get; set; }


}
}
