using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class NewWareAttributeValuesQueryResponse : JdResponse{


         [XmlElement("resultset")]
public  		List<string>
  resultset { get; set; }


}
}
