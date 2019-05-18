using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class ArealimitReadFindAreaLimitsByWareIdResponse : JdResponse{


         [XmlElement("wareAreaLimitList")]
public  		List<string>
  wareAreaLimitList { get; set; }


}
}
