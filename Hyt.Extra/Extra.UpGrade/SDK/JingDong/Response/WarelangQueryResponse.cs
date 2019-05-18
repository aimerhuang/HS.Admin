using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WarelangQueryResponse : JdResponse{


         [XmlElement("querywarelang_result")]
public  		string
  querywarelangResult { get; set; }


}
}
