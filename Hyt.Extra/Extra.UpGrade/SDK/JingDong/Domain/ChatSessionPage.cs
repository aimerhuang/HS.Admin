using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ChatSessionPage : JdObject{


         [XmlElement("chatSessionList")]
public  		List<string>
  chatSessionList { get; set; }


         [XmlElement("totalRecord")]
public  		string
  totalRecord { get; set; }


}
}
