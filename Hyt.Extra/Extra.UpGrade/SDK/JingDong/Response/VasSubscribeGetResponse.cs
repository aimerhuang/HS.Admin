using System;
using System.Xml.Serialization;
using System.Collections.Generic;

							namespace Extra.UpGrade.SDK.JingDong.Response
{





public class VasSubscribeGetResponse : JdResponse{


         [XmlElement("item_code")]
public  		string
  itemCode { get; set; }


         [XmlElement("end_date")]
public  		string
  endDate { get; set; }


}
}
