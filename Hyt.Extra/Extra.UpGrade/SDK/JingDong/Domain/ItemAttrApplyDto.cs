using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ItemAttrApplyDto : JdObject{


         [XmlElement("group_id")]
public  		string
  groupId { get; set; }


         [XmlElement("public_name")]
public  		string
  publicName { get; set; }


         [XmlElement("item_attr_detail_list")]
public  		List<string>
  itemAttrDetailList { get; set; }


}
}
