using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class DropshipDpsOutboundResponse : JdResponse{


         [XmlElement("outBoundResult")]
public  		string
  outBoundResult { get; set; }


}
}
