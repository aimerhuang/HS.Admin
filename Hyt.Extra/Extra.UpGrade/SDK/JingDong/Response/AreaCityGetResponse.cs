using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class AreaCityGetResponse : JdResponse{


         [XmlElement("city_areas")]
public  		string
  cityAreas { get; set; }


         [XmlElement("success")]
public  		bool
  success { get; set; }


}
}
