using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WareProductimageGetResponse : JdResponse{


         [XmlElement("image_path_list")]
public  		List<string>
  imagePathList { get; set; }


}
}
