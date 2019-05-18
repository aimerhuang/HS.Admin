using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QualificationDto : JdObject{


         [XmlElement("qualification_name")]
public  		string
  qualificationName { get; set; }


         [XmlElement("type")]
public  		int
  type { get; set; }


         [XmlElement("applicant")]
public  		string
  applicant { get; set; }


         [XmlElement("qc_code")]
public  		string
  qcCode { get; set; }


         [XmlElement("end_date")]
public  		DateTime
  endDate { get; set; }


         [XmlElement("qualification_files")]
public  		List<string>
  qualificationFiles { get; set; }


}
}
