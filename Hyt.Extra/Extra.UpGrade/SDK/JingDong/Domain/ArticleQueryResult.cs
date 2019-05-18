using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ArticleQueryResult : JdObject{


         [XmlElement("total_item")]
public  		string
  totalItem { get; set; }


         [XmlElement("total_page")]
public  		string
  totalPage { get; set; }


         [XmlElement("page_size")]
public  		string
  pageSize { get; set; }


         [XmlElement("page_index")]
public  		string
  pageIndex { get; set; }


         [XmlElement("article_list")]
public  		List<string>
  articleList { get; set; }


}
}
