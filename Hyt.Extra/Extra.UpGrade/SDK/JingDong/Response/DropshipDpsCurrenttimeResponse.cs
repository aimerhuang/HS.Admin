using System;
using System.Xml.Serialization;
using System.Collections.Generic;

				namespace Extra.UpGrade.SDK.JingDong.Response
{





public class DropshipDpsCurrenttimeResponse : JdResponse{


         [XmlElement("currentTime")]
public  		DateTime
  currentTime { get; set; }


}
}
