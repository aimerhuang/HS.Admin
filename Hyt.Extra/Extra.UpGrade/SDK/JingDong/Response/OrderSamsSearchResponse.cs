using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class OrderSamsSearchResponse : JdResponse{


         [XmlElement("searchsamorderinfo_result")]
public  		string
  searchsamorderinfoResult { get; set; }


}
}
