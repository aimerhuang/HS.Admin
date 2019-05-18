using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class PopOrderGetResponse : JdResponse{


         [XmlElement("orderDetailInfo")]
public  		string
  orderDetailInfo { get; set; }


}
}
