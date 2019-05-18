using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PageInfo : JdObject{


         [XmlElement("page_index")]
public  		long
  pageIndex { get; set; }


         [XmlElement("page_total")]
public  		long
  pageTotal { get; set; }


         [XmlElement("page_size")]
public  		long
  pageSize { get; set; }


         [XmlElement("datas")]
public  		List<string>
  datas { get; set; }


}
}
