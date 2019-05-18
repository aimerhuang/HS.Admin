using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class EdiRoDetailBatchGetResponse : JdResponse{


         [XmlElement("returnOrderLineBatchResultDTO")]
public  		string
  returnOrderLineBatchResultDTO { get; set; }


}
}
