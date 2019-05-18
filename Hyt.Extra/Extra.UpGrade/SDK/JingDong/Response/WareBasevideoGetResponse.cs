using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WareBasevideoGetResponse : JdResponse{


         [XmlElement("videoEntitys")]
public  		List<string>
  videoEntitys { get; set; }


}
}
