using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class KuaicheZnSpacePageByTypeSearchResponse : JdResponse{


         [XmlElement("space_page_info_list")]
public  		List<string>
  spacePageInfoList { get; set; }


}
}
