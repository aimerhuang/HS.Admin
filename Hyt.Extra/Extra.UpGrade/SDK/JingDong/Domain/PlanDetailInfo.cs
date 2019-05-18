using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PlanDetailInfo : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("space_id")]
public  		long
  spaceId { get; set; }


         [XmlElement("type")]
public  		int
  type { get; set; }


         [XmlElement("budget")]
public  		string
  budget { get; set; }


         [XmlElement("total_budget")]
public  		string
  totalBudget { get; set; }


         [XmlElement("mode")]
public  		int
  mode { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("allow_split")]
public  		int
  allowSplit { get; set; }


         [XmlElement("schedule_start")]
public  		string
  scheduleStart { get; set; }


         [XmlElement("schedule_end")]
public  		string
  scheduleEnd { get; set; }


         [XmlElement("insert_time")]
public  		string
  insertTime { get; set; }


         [XmlElement("update_time")]
public  		string
  updateTime { get; set; }


         [XmlElement("submit_time")]
public  		string
  submitTime { get; set; }


         [XmlElement("show_day")]
public  		string
  showDay { get; set; }


         [XmlElement("show_type")]
public  		int
  showType { get; set; }


         [XmlElement("material_list")]
public  		List<string>
  materialList { get; set; }


         [XmlElement("keyword_list")]
public  		List<string>
  keywordList { get; set; }


         [XmlElement("space_page_vo")]
public  		string
  spacePageVo { get; set; }


         [XmlElement("space_vo")]
public  		string
  spaceVo { get; set; }


}
}
