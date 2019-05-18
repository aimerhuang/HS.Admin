using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EptWarecenterWarelistGetResponse : JdResponse{


         [XmlElement("querywarelist_result")]
public  		string
  querywarelistResult { get; set; }


}
}
