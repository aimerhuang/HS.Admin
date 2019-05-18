using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class PoListPageGetResponse : JdResponse{


         [XmlElement("orderResultDto")]
public  		string
  orderResultDto { get; set; }


}
}
