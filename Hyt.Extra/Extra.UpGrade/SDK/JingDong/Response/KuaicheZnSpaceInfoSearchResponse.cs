using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class KuaicheZnSpaceInfoSearchResponse : JdResponse{


         [XmlElement("space_info_list")]
public  		List<string>
  spaceInfoList { get; set; }


}
}
