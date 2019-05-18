using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class VcLibraryGetUploadTokenResponse : JdResponse{


         [XmlElement("update_token")]
public  		string
  updateToken { get; set; }


}
}
