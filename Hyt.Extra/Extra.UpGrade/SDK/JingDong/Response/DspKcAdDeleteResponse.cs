using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class DspKcAdDeleteResponse : JdResponse{


         [XmlElement("delete_result")]
public  		string
  deleteResult { get; set; }


}
}
