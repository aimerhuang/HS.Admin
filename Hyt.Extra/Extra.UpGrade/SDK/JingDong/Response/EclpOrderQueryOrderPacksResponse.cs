using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EclpOrderQueryOrderPacksResponse : JdResponse{


         [XmlElement("queryorderpacks_result")]
public  		List<string>
  queryorderpacksResult { get; set; }


}
}
