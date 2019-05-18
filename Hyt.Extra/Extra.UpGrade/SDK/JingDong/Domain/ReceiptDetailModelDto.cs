using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ReceiptDetailModelDto : JdObject{


         [XmlElement("receivingNo")]
public  		string
  receivingNo { get; set; }


         [XmlElement("skuNo")]
public  		string
  skuNo { get; set; }


         [XmlElement("skuName")]
public  		string
  skuName { get; set; }


         [XmlElement("receivedQty")]
public  		string
  receivedQty { get; set; }


         [XmlElement("lotNo")]
public  		string
  lotNo { get; set; }


         [XmlElement("productLevel")]
public  		string
  productLevel { get; set; }


}
}
