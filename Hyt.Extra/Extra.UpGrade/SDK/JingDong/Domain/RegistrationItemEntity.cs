using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class RegistrationItemEntity : JdObject{


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("skuId")]
public  		long
  skuId { get; set; }


         [XmlElement("skuName")]
public  		string
  skuName { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("sex")]
public  		string
  sex { get; set; }


         [XmlElement("birthday")]
public  		DateTime
  birthday { get; set; }


         [XmlElement("idNumber")]
public  		string
  idNumber { get; set; }


         [XmlElement("nationality")]
public  		string
  nationality { get; set; }


         [XmlElement("homeAddress")]
public  		string
  homeAddress { get; set; }


         [XmlElement("addressDetail")]
public  		string
  addressDetail { get; set; }


         [XmlElement("phoneNumber")]
public  		string
  phoneNumber { get; set; }


         [XmlElement("email")]
public  		string
  email { get; set; }


         [XmlElement("emergencyContact")]
public  		string
  emergencyContact { get; set; }


         [XmlElement("emergencyContactNumber")]
public  		string
  emergencyContactNumber { get; set; }


         [XmlElement("clothingSize")]
public  		string
  clothingSize { get; set; }


         [XmlElement("beastResult")]
public  		string
  beastResult { get; set; }


         [XmlElement("certificatePictureUrl")]
public  		string
  certificatePictureUrl { get; set; }


         [XmlElement("job")]
public  		string
  job { get; set; }


         [XmlElement("informationChannel")]
public  		string
  informationChannel { get; set; }


}
}
