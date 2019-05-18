using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AdApplyDto : JdObject{


         [XmlElement("apply_id")]
public  		string
  applyId { get; set; }


         [XmlElement("state")]
public  		int
  state { get; set; }


         [XmlElement("create_by")]
public  		string
  createBy { get; set; }


         [XmlElement("modify_by")]
public  		string
  modifyBy { get; set; }


         [XmlElement("create_time")]
public  		DateTime
  createTime { get; set; }


         [XmlElement("modify_time")]
public  		DateTime
  modifyTime { get; set; }


         [XmlElement("login_name")]
public  		string
  loginName { get; set; }


         [XmlElement("apply_time")]
public  		DateTime
  applyTime { get; set; }


         [XmlElement("ware_id")]
public  		string
  wareId { get; set; }


         [XmlElement("product_name")]
public  		string
  productName { get; set; }


         [XmlElement("brand_name")]
public  		string
  brandName { get; set; }


         [XmlElement("adword_new")]
public  		string
  adwordNew { get; set; }


         [XmlElement("adword_old")]
public  		string
  adwordOld { get; set; }


         [XmlElement("subadvertise_dtos")]
public  		List<string>
  subadvertiseDtos { get; set; }


         [XmlElement("is_gaea")]
public  		int
  isGaea { get; set; }


         [XmlElement("cid_name")]
public  		string
  cidName { get; set; }


         [XmlElement("adaudit_dto")]
public  		string
  adauditDto { get; set; }


}
}
