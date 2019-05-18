using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ReceiptModelDto : JdObject{


         [XmlElement("receiptNo")]
public  		string
  receiptNo { get; set; }


         [XmlElement("ownerNo")]
public  		string
  ownerNo { get; set; }


         [XmlElement("supplierNo")]
public  		string
  supplierNo { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("detailModelDtos")]
public  		List<string>
  detailModelDtos { get; set; }


}
}
