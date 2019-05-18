using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class AreasGetResponse : JdResponse{


         [XmlElement("code_areas")]
public  		string
  codeAreas { get; set; }


         [XmlElement("success")]
public  		bool
  success { get; set; }


}
}
