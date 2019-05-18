using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class BuytoBuyNewWids : JdObject{


         [XmlElement("adWord")]
public  		string
  adWord { get; set; }


         [XmlElement("isBook")]
public  		bool
  isBook { get; set; }


         [XmlElement("canFreeRead")]
public  		bool
  canFreeRead { get; set; }


         [XmlElement("endRemainTime")]
public  		string
  endRemainTime { get; set; }


         [XmlElement("imageUrl")]
public  		string
  imageUrl { get; set; }


         [XmlElement("jdPrice")]
public  		double
  jdPrice { get; set; }


         [XmlElement("martPrice")]
public  		double
  martPrice { get; set; }


         [XmlElement("startRemainTime")]
public  		string
  startRemainTime { get; set; }


         [XmlElement("skuId")]
public  		string
  skuId { get; set; }


         [XmlElement("wareName")]
public  		string
  wareName { get; set; }


         [XmlElement("Good")]
public  		string
  Good { get; set; }


}
}
