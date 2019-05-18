using System;
using System.Xml.Serialization;
using System.Collections.Generic;

															using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class ImgzoneImageQueryAllResponse : JdResponse{


         [XmlElement("total_num")]
public  		string
  totalNum { get; set; }


         [XmlElement("desc")]
public  		string
  desc { get; set; }


         [XmlElement("return_code")]
public  		string
  returnCode { get; set; }


         [XmlElement("scroll_id")]
public  		string
  scrollId { get; set; }


         [XmlElement("result")]
public  		List<string>
  result { get; set; }


}
}
