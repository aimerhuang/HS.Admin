using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class DspMediaResult : JdObject{


         [XmlElement("mediaName")]
public  		string
  mediaName { get; set; }


         [XmlElement("mediaType")]
public  		string
  mediaType { get; set; }


         [XmlElement("adPosition")]
public  		string
  adPosition { get; set; }


         [XmlElement("adType")]
public  		string
  adType { get; set; }


         [XmlElement("adScreenShot")]
public  		string
  adScreenShot { get; set; }


         [XmlElement("size")]
public  		string
  size { get; set; }


}
}
