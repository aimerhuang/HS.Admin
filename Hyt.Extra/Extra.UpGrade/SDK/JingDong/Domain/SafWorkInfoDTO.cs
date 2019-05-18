using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SafWorkInfoDTO : JdObject{


         [XmlElement("send_biztype_id")]
public  		long
  sendBiztypeId { get; set; }


         [XmlElement("send_biztype_name")]
public  		string
  sendBiztypeName { get; set; }


         [XmlElement("reply_biztype_name")]
public  		string
  replyBiztypeName { get; set; }


         [XmlElement("reply_sub_biztype_name")]
public  		string
  replySubBiztypeName { get; set; }


         [XmlElement("santisfaction")]
public  		string
  santisfaction { get; set; }


         [XmlElement("create_date")]
public  		DateTime
  createDate { get; set; }


         [XmlElement("oper_logs")]
public  		List<string>
  operLogs { get; set; }


}
}
