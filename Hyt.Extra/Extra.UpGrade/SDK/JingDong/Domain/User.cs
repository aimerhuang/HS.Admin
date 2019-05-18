using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class User : JdObject{


         [XmlElement("test_name")]
public  		string
  testName { get; set; }


         [XmlElement("test_telPhone")]
public  		string
  testTelPhone { get; set; }


         [XmlElement("test_mobile")]
public  		string
  testMobile { get; set; }


         [XmlElement("test_address")]
public  		string
  testAddress { get; set; }


         [XmlElement("test_email")]
public  		string
  testEmail { get; set; }


         [XmlElement("test_sendAdress")]
public  		string
  testSendAdress { get; set; }


         [XmlElement("test_reciveAdress")]
public  		string
  testReciveAdress { get; set; }


}
}
