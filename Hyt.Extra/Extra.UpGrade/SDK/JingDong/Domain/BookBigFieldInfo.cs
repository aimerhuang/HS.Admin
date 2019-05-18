using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class BookBigFieldInfo : JdObject{


         [XmlElement("comments")]
public  		string
  comments { get; set; }


         [XmlElement("image")]
public  		string
  image { get; set; }


         [XmlElement("content_desc")]
public  		string
  contentDesc { get; set; }


         [XmlElement("relatedProducts")]
public  		string
  relatedProducts { get; set; }


         [XmlElement("editer_desc")]
public  		string
  editerDesc { get; set; }


         [XmlElement("catalogue")]
public  		string
  catalogue { get; set; }


         [XmlElement("book_abstract")]
public  		string
  bookAbstract { get; set; }


         [XmlElement("authorDesc")]
public  		string
  authorDesc { get; set; }


         [XmlElement("introduction")]
public  		string
  introduction { get; set; }


         [XmlElement("productFeatures")]
public  		string
  productFeatures { get; set; }


}
}
