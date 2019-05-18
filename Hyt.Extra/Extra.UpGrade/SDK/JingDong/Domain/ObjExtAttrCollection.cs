using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ObjExtAttrCollection : JdObject{


         [XmlElement("expandsortid")]
public  		string
  expandsortid { get; set; }


         [XmlElement("expandsortname")]
public  		string
  expandsortname { get; set; }


         [XmlElement("sortorder")]
public  		string
  sortorder { get; set; }


         [XmlElement("valueid")]
public  		string
  valueid { get; set; }


         [XmlElement("valuename")]
public  		string
  valuename { get; set; }


}
}
