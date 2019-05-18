using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class MarketServiceQtSubscribeGetResponse : JdResponse{


         [XmlElement("qtArticleResult")]
public  		string
  qtArticleResult { get; set; }


}
}
