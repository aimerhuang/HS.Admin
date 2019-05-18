using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class KeywordQueryVO : JdObject{


         [XmlElement("third_categoryid")]
public  		long
  thirdCategoryid { get; set; }


         [XmlElement("sort_field")]
public  		string
  sortField { get; set; }


         [XmlElement("sort_type")]
public  		int
  sortType { get; set; }


         [XmlElement("total_number")]
public  		int
  totalNumber { get; set; }


         [XmlElement("page_size")]
public  		int
  pageSize { get; set; }


         [XmlElement("page_index")]
public  		int
  pageIndex { get; set; }


         [XmlElement("keyword_data")]
public  		List<string>
  keywordData { get; set; }


}
}
