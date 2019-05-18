using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PopCommentJosVo : JdObject{


         [XmlElement("commentId")]
public  		string
  commentId { get; set; }


         [XmlElement("skuid")]
public  		string
  skuid { get; set; }


         [XmlElement("content")]
public  		string
  content { get; set; }


         [XmlElement("creationTime")]
public  		DateTime
  creationTime { get; set; }


         [XmlElement("skuImage")]
public  		string
  skuImage { get; set; }


         [XmlElement("skuName")]
public  		string
  skuName { get; set; }


         [XmlElement("replyCount")]
public  		string
  replyCount { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("score")]
public  		string
  score { get; set; }


         [XmlElement("usefulCount")]
public  		string
  usefulCount { get; set; }


         [XmlElement("isVenderReply")]
public  		bool
  isVenderReply { get; set; }


         [XmlElement("nickName")]
public  		string
  nickName { get; set; }


         [XmlElement("replies")]
public  		List<string>
  replies { get; set; }


}
}
