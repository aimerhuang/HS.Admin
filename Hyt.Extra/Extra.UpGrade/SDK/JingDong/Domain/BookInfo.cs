using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class BookInfo : JdObject{


         [XmlElement("id")]
public  		string
  id { get; set; }


         [XmlElement("first_category")]
public  		int
  firstCategory { get; set; }


         [XmlElement("isbn")]
public  		string
  isbn { get; set; }


         [XmlElement("issn")]
public  		string
  issn { get; set; }


         [XmlElement("book_name")]
public  		string
  bookName { get; set; }


         [XmlElement("foreign_book_name")]
public  		string
  foreignBookName { get; set; }


         [XmlElement("language")]
public  		string
  language { get; set; }


         [XmlElement("author")]
public  		string
  author { get; set; }


         [XmlElement("editer")]
public  		string
  editer { get; set; }


         [XmlElement("proofreader")]
public  		string
  proofreader { get; set; }


         [XmlElement("remarker")]
public  		string
  remarker { get; set; }


         [XmlElement("transfer")]
public  		string
  transfer { get; set; }


         [XmlElement("drawer")]
public  		string
  drawer { get; set; }


         [XmlElement("publishers")]
public  		string
  publishers { get; set; }


         [XmlElement("publish_no")]
public  		string
  publishNo { get; set; }


         [XmlElement("series")]
public  		string
  series { get; set; }


         [XmlElement("brand")]
public  		string
  brand { get; set; }


         [XmlElement("format")]
public  		string
  format { get; set; }


         [XmlElement("packages")]
public  		string
  packages { get; set; }


         [XmlElement("pages")]
public  		string
  pages { get; set; }


         [XmlElement("batch_no")]
public  		string
  batchNo { get; set; }


         [XmlElement("publish_time")]
public  		string
  publishTime { get; set; }


         [XmlElement("print_no")]
public  		string
  printNo { get; set; }


         [XmlElement("print_time")]
public  		string
  printTime { get; set; }


         [XmlElement("size_and_height")]
public  		string
  sizeAndHeight { get; set; }


         [XmlElement("china_catalog")]
public  		string
  chinaCatalog { get; set; }


         [XmlElement("sheet")]
public  		string
  sheet { get; set; }


         [XmlElement("papers")]
public  		string
  papers { get; set; }


         [XmlElement("attachment")]
public  		string
  attachment { get; set; }


         [XmlElement("attachment_num")]
public  		string
  attachmentNum { get; set; }


         [XmlElement("pack_num")]
public  		string
  packNum { get; set; }


         [XmlElement("letters")]
public  		string
  letters { get; set; }


         [XmlElement("bar_code")]
public  		string
  barCode { get; set; }


         [XmlElement("keywords")]
public  		string
  keywords { get; set; }


         [XmlElement("pick_state")]
public  		string
  pickState { get; set; }


         [XmlElement("compile")]
public  		string
  compile { get; set; }


         [XmlElement("photography")]
public  		string
  photography { get; set; }


         [XmlElement("dictation")]
public  		string
  dictation { get; set; }


         [XmlElement("read")]
public  		string
  read { get; set; }


         [XmlElement("finishing")]
public  		string
  finishing { get; set; }


         [XmlElement("write")]
public  		string
  write { get; set; }


}
}
