using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PopCommentReplyVo : JdObject{


         [XmlElement("content")]
public  		string
  content { get; set; }


         [XmlElement("creationTime")]
public  		DateTime
  creationTime { get; set; }


         [XmlElement("nickName")]
public  		string
  nickName { get; set; }


         [XmlElement("replyId")]
public  		long
  replyId { get; set; }


}
}
