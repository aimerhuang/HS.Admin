using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class CrmModelGetResponse : JdResponse{


         [XmlElement("member_dynm_models")]
public  		string
  memberDynmModels { get; set; }


}
}
