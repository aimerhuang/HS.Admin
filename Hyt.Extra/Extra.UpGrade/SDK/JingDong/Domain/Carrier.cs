using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Carrier : JdObject{


         [XmlElement("id")]
public  		int
  id { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("type")]
public  		int
  type { get; set; }


         [XmlElement("description")]
public  		string
  description { get; set; }


         [XmlElement("agreeFlag")]
public  		string
  agreeFlag { get; set; }


         [XmlElement("useFlag")]
public  		string
  useFlag { get; set; }


         [XmlElement("sort")]
public  		int
  sort { get; set; }


         [XmlElement("comments")]
public  		string
  comments { get; set; }


         [XmlElement("printFlag")]
public  		string
  printFlag { get; set; }


         [XmlElement("templateName")]
public  		string
  templateName { get; set; }


         [XmlElement("pin")]
public  		string
  pin { get; set; }


         [XmlElement("jdAccount")]
public  		string
  jdAccount { get; set; }


         [XmlElement("link")]
public  		string
  link { get; set; }


         [XmlElement("score")]
public  		double
  score { get; set; }


         [XmlElement("isDefault")]
public  		int
  isDefault { get; set; }


}
}
