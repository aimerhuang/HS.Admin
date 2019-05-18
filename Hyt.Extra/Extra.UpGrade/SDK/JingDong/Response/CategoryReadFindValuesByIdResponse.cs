using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class CategoryReadFindValuesByIdResponse : JdResponse{


         [XmlElement("categoryAttrValue")]
public  		string
  categoryAttrValue { get; set; }


}
}
