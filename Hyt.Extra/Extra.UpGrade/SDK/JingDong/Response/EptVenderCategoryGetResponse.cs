using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EptVenderCategoryGetResponse : JdResponse{


         [XmlElement("getvendercategory_result")]
public  		string
  getvendercategoryResult { get; set; }


}
}