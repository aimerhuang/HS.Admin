using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EclpOrderQueryOrderStatusResponse : JdResponse{


         [XmlElement("queryorderstatus_result")]
public  		string
  queryorderstatusResult { get; set; }


}
}
