using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PickUpResultDTO : JdObject{


         [XmlElement("code")]
public  		int
  code { get; set; }


         [XmlElement("messsage")]
public  		string
  messsage { get; set; }


         [XmlElement("pickUpCode")]
public  		string
  pickUpCode { get; set; }


}
}
