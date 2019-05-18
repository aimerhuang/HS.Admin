using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EpsEditCheckStatusResponse : JdResponse{


         [XmlElement("editcheckstatus_result")]
public  		string
  editcheckstatusResult { get; set; }


}
}
