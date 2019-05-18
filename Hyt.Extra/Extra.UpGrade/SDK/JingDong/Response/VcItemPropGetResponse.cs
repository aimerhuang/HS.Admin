using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class VcItemPropGetResponse : JdResponse{


         [XmlElement("props")]
public  		List<string>
  props { get; set; }


}
}
