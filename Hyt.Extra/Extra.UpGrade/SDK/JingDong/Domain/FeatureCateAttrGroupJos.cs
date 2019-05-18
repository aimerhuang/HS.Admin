using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class FeatureCateAttrGroupJos : JdObject{


         [XmlElement("attrGroupFeatureCn")]
public  		string
  attrGroupFeatureCn { get; set; }


         [XmlElement("attrGroupFeatureKey")]
public  		string
  attrGroupFeatureKey { get; set; }


         [XmlElement("attrGroupFeatureValue")]
public  		string
  attrGroupFeatureValue { get; set; }


}
}
