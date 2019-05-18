using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class CategoryReadFindValuesByAttrIdResponse : JdResponse{


         [XmlElement("categoryAttrValues")]
public  		List<string>
  categoryAttrValues { get; set; }


}
}
