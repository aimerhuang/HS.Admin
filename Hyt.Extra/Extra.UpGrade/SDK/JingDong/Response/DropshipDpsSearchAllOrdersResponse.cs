using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class DropshipDpsSearchAllOrdersResponse : JdResponse{


         [XmlElement("searchallorders_result")]
public  		string
  searchallordersResult { get; set; }


}
}
