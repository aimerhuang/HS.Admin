using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class CrmModelMemberGetResponse : JdResponse{


         [XmlElement("crm_member_result")]
public  		string
  crmMemberResult { get; set; }


}
}
