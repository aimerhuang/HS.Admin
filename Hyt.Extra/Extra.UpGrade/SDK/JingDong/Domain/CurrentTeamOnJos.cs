using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CurrentTeamOnJos : JdObject{


         [XmlElement("id")]
public  		string
  id { get; set; }


         [XmlElement("city_id")]
public  		string
  cityId { get; set; }


         [XmlElement("city_name")]
public  		string
  cityName { get; set; }


         [XmlElement("group_id")]
public  		string
  groupId { get; set; }


         [XmlElement("group_name")]
public  		string
  groupName { get; set; }


         [XmlElement("group_url")]
public  		string
  groupUrl { get; set; }


         [XmlElement("title")]
public  		string
  title { get; set; }


         [XmlElement("product")]
public  		string
  product { get; set; }


         [XmlElement("market_price")]
public  		string
  marketPrice { get; set; }


         [XmlElement("team_price")]
public  		string
  teamPrice { get; set; }


         [XmlElement("begin_time")]
public  		string
  beginTime { get; set; }


         [XmlElement("end_time")]
public  		string
  endTime { get; set; }


         [XmlElement("coupon_expire_time")]
public  		string
  couponExpireTime { get; set; }


         [XmlElement("min_number")]
public  		string
  minNumber { get; set; }


         [XmlElement("max_number")]
public  		string
  maxNumber { get; set; }


         [XmlElement("per_number")]
public  		string
  perNumber { get; set; }


         [XmlElement("now_number")]
public  		string
  nowNumber { get; set; }


         [XmlElement("team_image")]
public  		string
  teamImage { get; set; }


         [XmlElement("external_url")]
public  		string
  externalUrl { get; set; }


         [XmlElement("sort_order")]
public  		string
  sortOrder { get; set; }


         [XmlElement("detail_url")]
public  		string
  detailUrl { get; set; }


         [XmlElement("team_zip_image")]
public  		string
  teamZipImage { get; set; }


         [XmlElement("group2_list")]
public  		List<string>
  group2List { get; set; }


         [XmlElement("district_area_list")]
public  		List<string>
  districtAreaList { get; set; }


}
}
