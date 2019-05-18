using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class SearchWareResponse : JdResponse{


         [XmlElement("Summary")]
public  		string
  Summary { get; set; }


         [XmlElement("ObjA_Price")]
public  		List<string>
  objAPrice { get; set; }


         [XmlElement("ObjExtAttrCollection")]
public  		List<string>
  ObjExtAttrCollection { get; set; }


         [XmlElement("Paragraph")]
public  		List<string>
  Paragraph { get; set; }


}
}
