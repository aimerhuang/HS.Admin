using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PublicResultObjectWaitFetch : JdObject{


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("resultErrorMsg")]
public  		string
  resultErrorMsg { get; set; }


         [XmlElement("waitFetch")]
public  		string
  waitFetch { get; set; }


}
}
