using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EdiSpiLargeGetResponse : JdResponse{


         [XmlElement("largeSparePartInventoryResult")]
public  		string
  largeSparePartInventoryResult { get; set; }


}
}
