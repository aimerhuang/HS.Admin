using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class UploadResponse : JdObject{


         [XmlElement("resultCode")]
public  		string
  resultCode { get; set; }


         [XmlElement("resultMsg")]
public  		string
  resultMsg { get; set; }


}
}
