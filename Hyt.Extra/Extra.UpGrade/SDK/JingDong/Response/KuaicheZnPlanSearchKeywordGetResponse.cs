using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class KuaicheZnPlanSearchKeywordGetResponse : JdResponse{


         [XmlElement("keywords_info")]
public  		List<string>
  keywordsInfo { get; set; }


}
}