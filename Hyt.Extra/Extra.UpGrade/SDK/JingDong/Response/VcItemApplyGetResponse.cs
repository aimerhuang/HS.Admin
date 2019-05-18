using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class VcItemApplyGetResponse : JdResponse{


         [XmlElement("apply_info")]
public  		string
  applyInfo { get; set; }


}
}
