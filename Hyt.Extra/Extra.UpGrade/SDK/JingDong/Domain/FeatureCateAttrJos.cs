using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class FeatureCateAttrJos : JdObject{


         [XmlElement("attrFeatureCn")]
public  		string
  attrFeatureCn { get; set; }


         [XmlElement("attrFeatureKey")]
public  		string
  attrFeatureKey { get; set; }


         [XmlElement("attrFeatureValue")]
public  		string
  attrFeatureValue { get; set; }


}
}
