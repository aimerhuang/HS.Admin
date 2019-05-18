using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class VcItemPurchaserGetResponse : JdResponse{


         [XmlElement("purchaser")]
public  		string
  purchaser { get; set; }


}
}
