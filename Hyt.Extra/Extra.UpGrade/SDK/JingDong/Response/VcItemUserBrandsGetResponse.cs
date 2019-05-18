using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class VcItemUserBrandsGetResponse : JdResponse{


         [XmlElement("userBrands")]
public  		List<string>
  userBrands { get; set; }


}
}
