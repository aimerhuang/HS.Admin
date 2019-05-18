using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WaitProcessPageExport : JdObject{


         [XmlElement("totalNum")]
public  		string
  totalNum { get; set; }


         [XmlElement("waitProcessExportList")]
public  		List<string>
  waitProcessExportList { get; set; }


}
}
