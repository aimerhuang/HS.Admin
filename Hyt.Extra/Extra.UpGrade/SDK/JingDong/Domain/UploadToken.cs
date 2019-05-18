using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class UploadToken : JdObject{


         [XmlElement("upload_url")]
public  		string
  uploadUrl { get; set; }


         [XmlElement("start_time")]
public  		string
  startTime { get; set; }


}
}
