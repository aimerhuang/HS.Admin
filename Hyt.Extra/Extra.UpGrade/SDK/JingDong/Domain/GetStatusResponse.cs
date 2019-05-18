using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class GetStatusResponse : JdObject{


         [XmlElement("resultCode")]
public  		string
  resultCode { get; set; }


         [XmlElement("column_list")]
public  		string
  columnList { get; set; }


         [XmlElement("row_list")]
public  		List<string>
  rowList { get; set; }


         [XmlElement("resultMsg")]
public  		string
  resultMsg { get; set; }


}
}
