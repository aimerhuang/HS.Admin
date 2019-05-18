using System;
using System.Xml.Serialization;
using System.Collections.Generic;

									using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class ImgzoneCategoryQueryResponse : JdResponse{


         [XmlElement("return_code")]
public  		string
  returnCode { get; set; }


         [XmlElement("desc1")]
public  		string
  desc1 { get; set; }


         [XmlElement("cateList")]
public  		List<string>
  cateList { get; set; }


}
}
