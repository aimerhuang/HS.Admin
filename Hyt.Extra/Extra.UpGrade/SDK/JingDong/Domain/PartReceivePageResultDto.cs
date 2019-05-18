using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PartReceivePageResultDto : JdObject{


         [XmlElement("totalNum")]
public  		string
  totalNum { get; set; }


         [XmlElement("pageSize")]
public  		int
  pageSize { get; set; }


         [XmlElement("pageNumer")]
public  		int
  pageNumer { get; set; }


         [XmlElement("partReceiveDtoList")]
public  		List<string>
  partReceiveDtoList { get; set; }


}
}
