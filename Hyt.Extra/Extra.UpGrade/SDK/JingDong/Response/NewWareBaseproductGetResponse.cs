using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class NewWareBaseproductGetResponse : JdResponse{


         [XmlElement("listproductbase_result")]
public  		List<string>
  listproductbaseResult { get; set; }


}
}
