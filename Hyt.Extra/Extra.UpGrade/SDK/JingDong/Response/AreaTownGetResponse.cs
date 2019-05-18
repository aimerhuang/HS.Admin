using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class AreaTownGetResponse : JdResponse{


         [XmlElement("town_areas")]
public  		string
  townAreas { get; set; }


         [XmlElement("success")]
public  		bool
  success { get; set; }


}
}
