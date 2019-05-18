using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SupplierModel : JdObject{


         [XmlElement("deptNo")]
public  		string
  deptNo { get; set; }


         [XmlElement("deptName")]
public  		string
  deptName { get; set; }


         [XmlElement("eclpSupplierNo")]
public  		string
  eclpSupplierNo { get; set; }


         [XmlElement("supplierName")]
public  		string
  supplierName { get; set; }


         [XmlElement("supplierType")]
public  		string
  supplierType { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("contacts")]
public  		string
  contacts { get; set; }


         [XmlElement("phone")]
public  		string
  phone { get; set; }


         [XmlElement("fax")]
public  		string
  fax { get; set; }


         [XmlElement("email")]
public  		string
  email { get; set; }


         [XmlElement("province")]
public  		string
  province { get; set; }


         [XmlElement("city")]
public  		string
  city { get; set; }


         [XmlElement("county")]
public  		string
  county { get; set; }


         [XmlElement("town")]
public  		string
  town { get; set; }


         [XmlElement("address")]
public  		string
  address { get; set; }


         [XmlElement("ext1")]
public  		string
  ext1 { get; set; }


         [XmlElement("ext2")]
public  		string
  ext2 { get; set; }


         [XmlElement("ext3")]
public  		string
  ext3 { get; set; }


         [XmlElement("ext4")]
public  		string
  ext4 { get; set; }


         [XmlElement("ext5")]
public  		string
  ext5 { get; set; }


}
}
