using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ChatLogPage : JdObject{


         [XmlElement("chatLogList")]
public  		List<string>
  chatLogList { get; set; }


         [XmlElement("totalRecord")]
public  		string
  totalRecord { get; set; }


}
}
