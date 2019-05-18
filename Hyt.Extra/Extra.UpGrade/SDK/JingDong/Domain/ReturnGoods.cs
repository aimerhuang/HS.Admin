using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ReturnGoods : JdObject{


         [XmlElement("total_num")]
public  		string
  totalNum { get; set; }


         [XmlElement("return_infos")]
public  		List<string>
  returnInfos { get; set; }


}
}
