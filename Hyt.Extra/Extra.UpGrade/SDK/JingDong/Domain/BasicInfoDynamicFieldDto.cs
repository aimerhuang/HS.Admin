using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class BasicInfoDynamicFieldDto : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("cid3")]
public  		int
  cid3 { get; set; }


         [XmlElement("field_id")]
public  		string
  fieldId { get; set; }


         [XmlElement("field_name")]
public  		string
  fieldName { get; set; }


         [XmlElement("field_length")]
public  		string
  fieldLength { get; set; }


         [XmlElement("field_value")]
public  		string
  fieldValue { get; set; }


         [XmlElement("field_type")]
public  		int
  fieldType { get; set; }


         [XmlElement("is_necessary")]
public  		int
  isNecessary { get; set; }


         [XmlElement("is_show")]
public  		int
  isShow { get; set; }


         [XmlElement("offset")]
public  		int
  offset { get; set; }


         [XmlElement("limit")]
public  		int
  limit { get; set; }


}
}
