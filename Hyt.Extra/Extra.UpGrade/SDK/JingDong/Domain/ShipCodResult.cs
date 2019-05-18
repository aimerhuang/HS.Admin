using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ShipCodResult : JdObject{


         [XmlElement("supportJdShip")]
public  		bool
  supportJdShip { get; set; }


         [XmlElement("supportFreshShip")]
public  		bool
  supportFreshShip { get; set; }


         [XmlElement("supportJdCod")]
public  		bool
  supportJdCod { get; set; }


         [XmlElement("supportJdPos")]
public  		bool
  supportJdPos { get; set; }


         [XmlElement("supportJd3Cod")]
public  		bool
  supportJd3Cod { get; set; }


         [XmlElement("supportSpecialDelivery")]
public  		bool
  supportSpecialDelivery { get; set; }


         [XmlElement("supportCold1")]
public  		bool
  supportCold1 { get; set; }


         [XmlElement("supportCold2")]
public  		bool
  supportCold2 { get; set; }


         [XmlElement("supportCold3")]
public  		bool
  supportCold3 { get; set; }


         [XmlElement("supportCold4")]
public  		bool
  supportCold4 { get; set; }


         [XmlElement("supportDirect")]
public  		bool
  supportDirect { get; set; }


         [XmlElement("supportPickup")]
public  		bool
  supportPickup { get; set; }


         [XmlElement("supportHKMOShip")]
public  		bool
  supportHKMOShip { get; set; }


         [XmlElement("unSupportGAShipProducts")]
public  		string
  unSupportGAShipProducts { get; set; }


         [XmlElement("unSupportFreshShipProducts")]
public  		string
  unSupportFreshShipProducts { get; set; }


         [XmlElement("shipType")]
public  		int
  shipType { get; set; }


         [XmlElement("errorMsg")]
public  		string
  errorMsg { get; set; }


}
}
