using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class AreaProvinceGetResponse : JdResponse{


         [XmlElement("province_areas")]
public  		string
  provinceAreas { get; set; }


         [XmlElement("success")]
public  		bool
  success { get; set; }


}
}
