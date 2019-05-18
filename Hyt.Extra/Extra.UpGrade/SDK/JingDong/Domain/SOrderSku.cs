using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SOrderSku : JdObject{


         [XmlElement("skuId")]
public  		long
  skuId { get; set; }


         [XmlElement("skuName")]
public  		string
  skuName { get; set; }


         [XmlElement("skuNum")]
public  		long
  skuNum { get; set; }


         [XmlElement("sequenceId")]
public  		string
  sequenceId { get; set; }


         [XmlElement("outerId")]
public  		string
  outerId { get; set; }


         [XmlElement("price")]
public  		string
  price { get; set; }


}
}
