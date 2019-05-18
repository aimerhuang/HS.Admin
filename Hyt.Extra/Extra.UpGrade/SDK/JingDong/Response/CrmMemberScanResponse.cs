using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class CrmMemberScanResponse : JdResponse{


         [XmlElement("crm_member_scan_result")]
public  		string
  crmMemberScanResult { get; set; }


}
}
