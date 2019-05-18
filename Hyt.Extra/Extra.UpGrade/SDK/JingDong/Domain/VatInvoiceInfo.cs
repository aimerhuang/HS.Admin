using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VatInvoiceInfo : JdObject{


         [XmlElement("taxpayer_ident")]
public  		string
  taxpayerIdent { get; set; }


         [XmlElement("registered_address")]
public  		string
  registeredAddress { get; set; }


         [XmlElement("registered_phone")]
public  		string
  registeredPhone { get; set; }


         [XmlElement("deposit_bank")]
public  		string
  depositBank { get; set; }


         [XmlElement("bank_account")]
public  		string
  bankAccount { get; set; }


}
}
