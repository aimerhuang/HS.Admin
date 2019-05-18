using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ServicePageExport : JdObject{


         [XmlElement("totalNum")]
public  		string
  totalNum { get; set; }


         [XmlElement("pageSize")]
public  		int
  pageSize { get; set; }


         [XmlElement("pageNumer")]
public  		int
  pageNumer { get; set; }


         [XmlElement("serviceExportList")]
public  		List<string>
  serviceExportList { get; set; }


}
}
