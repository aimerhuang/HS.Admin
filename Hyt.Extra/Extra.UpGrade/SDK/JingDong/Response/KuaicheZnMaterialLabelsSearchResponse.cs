using System;
using System.Xml.Serialization;
using System.Collections.Generic;

			using Extra.UpGrade.SDK.JingDong.Domain;
			namespace Extra.UpGrade.SDK.JingDong.Response
{





public class KuaicheZnMaterialLabelsSearchResponse : JdResponse{


         [XmlElement("material_label_list")]
public  		List<string>
  materialLabelList { get; set; }


}
}
