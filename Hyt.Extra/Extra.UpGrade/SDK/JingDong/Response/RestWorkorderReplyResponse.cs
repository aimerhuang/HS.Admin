using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class RestWorkorderReplyResponse : JdResponse{


         [XmlElement("work_order")]
public  		string
  workOrder { get; set; }


}
}
