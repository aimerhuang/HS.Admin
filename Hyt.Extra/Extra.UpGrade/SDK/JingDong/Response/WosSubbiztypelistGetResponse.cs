using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WosSubbiztypelistGetResponse : JdResponse{


         [XmlElement("saf_biztype_Dtos")]
public  		List<string>
  safBiztypeDtos { get; set; }


}
}
