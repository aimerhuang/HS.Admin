using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class DownloadResponse : JdObject{


         [XmlElement("resultCode")]
public  		string
  resultCode { get; set; }


         [XmlElement("columnList")]
public  		List<string>
  columnList { get; set; }


         [XmlElement("dataList")]
public  		List<string>
  dataList { get; set; }


         [XmlElement("resultMsg")]
public  		string
  resultMsg { get; set; }


}
}
