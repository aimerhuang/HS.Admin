using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class DspAdunitStatusUpdateResponse : JdResponse{


         [XmlElement("updatastatus_result")]
public  		string
  updatastatusResult { get; set; }


}
}
