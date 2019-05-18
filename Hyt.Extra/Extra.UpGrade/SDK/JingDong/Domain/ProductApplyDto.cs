using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ProductApplyDto : JdObject{


         [XmlElement("apply_id")]
public  		string
  applyId { get; set; }


         [XmlElement("created_by")]
public  		string
  createdBy { get; set; }


         [XmlElement("created_time")]
public  		DateTime
  createdTime { get; set; }


         [XmlElement("modified_by")]
public  		string
  modifiedBy { get; set; }


         [XmlElement("modified_time")]
public  		DateTime
  modifiedTime { get; set; }


         [XmlElement("apply_time")]
public  		DateTime
  applyTime { get; set; }


         [XmlElement("state")]
public  		int
  state { get; set; }


         [XmlElement("archive_status")]
public  		int
  archiveStatus { get; set; }


         [XmlElement("yn")]
public  		int
  yn { get; set; }


         [XmlElement("product_type")]
public  		int
  productType { get; set; }


         [XmlElement("product_info")]
public  		string
  productInfo { get; set; }


         [XmlElement("current_audit_info")]
public  		string
  currentAuditInfo { get; set; }


         [XmlElement("audit_records")]
public  		List<string>
  auditRecords { get; set; }


}
}
