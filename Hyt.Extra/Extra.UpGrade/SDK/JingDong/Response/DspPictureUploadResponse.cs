using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class DspPictureUploadResponse : JdResponse{


         [XmlElement("uploadPic_result")]
public  		string
  uploadPicResult { get; set; }


}
}
