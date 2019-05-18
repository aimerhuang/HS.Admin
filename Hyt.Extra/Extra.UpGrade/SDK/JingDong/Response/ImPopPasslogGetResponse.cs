using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class ImPopPasslogGetResponse : JdResponse{


         [XmlElement("PassLog")]
public  		List<string>
  PassLog { get; set; }


}
}
