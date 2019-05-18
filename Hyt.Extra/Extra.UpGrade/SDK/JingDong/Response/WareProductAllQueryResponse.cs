using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WareProductAllQueryResponse : JdResponse{


         [XmlElement("queryskuallbycondition_result")]
public  		string
  queryskuallbyconditionResult { get; set; }


}
}
