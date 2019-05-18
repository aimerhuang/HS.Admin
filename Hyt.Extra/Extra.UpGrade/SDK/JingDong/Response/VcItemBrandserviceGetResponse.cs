using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class VcItemBrandserviceGetResponse : JdResponse{


         [XmlElement("brand_service")]
public  		string
  brandService { get; set; }


}
}
