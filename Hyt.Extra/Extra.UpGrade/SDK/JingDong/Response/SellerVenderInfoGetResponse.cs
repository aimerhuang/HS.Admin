using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class SellerVenderInfoGetResponse : JdResponse{


         [XmlElement("vender_info_result")]
public  		string
  venderInfoResult { get; set; }


}
}
