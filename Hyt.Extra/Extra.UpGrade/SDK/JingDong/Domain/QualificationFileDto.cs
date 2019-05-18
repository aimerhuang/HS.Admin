using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QualificationFileDto : JdObject{


         [XmlElement("file_name")]
public  		string
  fileName { get; set; }


         [XmlElement("file_key")]
public  		string
  fileKey { get; set; }


         [XmlElement("file_path")]
public  		string
  filePath { get; set; }


         [XmlElement("file_size")]
public  		long
  fileSize { get; set; }


         [XmlElement("file_type")]
public  		string
  fileType { get; set; }


}
}
