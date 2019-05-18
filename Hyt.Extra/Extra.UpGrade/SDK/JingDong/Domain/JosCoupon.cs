using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosCoupon : JdObject{


         [XmlElement("couponId")]
public  		long
  couponId { get; set; }


         [XmlElement("venderId")]
public  		long
  venderId { get; set; }


         [XmlElement("lockType")]
public  		int
  lockType { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("type")]
public  		int
  type { get; set; }


         [XmlElement("bindType")]
public  		int
  bindType { get; set; }


         [XmlElement("grantType")]
public  		int
  grantType { get; set; }


         [XmlElement("num")]
public  		int
  num { get; set; }


         [XmlElement("discount")]
public  		string
  discount { get; set; }


         [XmlElement("quota")]
public  		string
  quota { get; set; }


         [XmlElement("validityType")]
public  		int
  validityType { get; set; }


         [XmlElement("days")]
public  		int
  days { get; set; }


         [XmlElement("beginTime")]
public  		long
  beginTime { get; set; }


         [XmlElement("endTime")]
public  		long
  endTime { get; set; }


         [XmlElement("password")]
public  		string
  password { get; set; }


         [XmlElement("batchKey")]
public  		string
  batchKey { get; set; }


         [XmlElement("rfId")]
public  		long
  rfId { get; set; }


         [XmlElement("member")]
public  		int
  member { get; set; }


         [XmlElement("takeBeginTime")]
public  		long
  takeBeginTime { get; set; }


         [XmlElement("takeEndTime")]
public  		long
  takeEndTime { get; set; }


         [XmlElement("takeRule")]
public  		int
  takeRule { get; set; }


         [XmlElement("takeNum")]
public  		int
  takeNum { get; set; }


         [XmlElement("link")]
public  		string
  link { get; set; }


         [XmlElement("activityRfId")]
public  		long
  activityRfId { get; set; }


         [XmlElement("activityLink")]
public  		string
  activityLink { get; set; }


         [XmlElement("usedNum")]
public  		int
  usedNum { get; set; }


         [XmlElement("sendNum")]
public  		int
  sendNum { get; set; }


         [XmlElement("deleted")]
public  		bool
  deleted { get; set; }


         [XmlElement("display")]
public  		int
  display { get; set; }


         [XmlElement("created")]
public  		long
  created { get; set; }


         [XmlElement("platformType")]
public  		int
  platformType { get; set; }


         [XmlElement("platform")]
public  		string
  platform { get; set; }


         [XmlElement("imgUrl")]
public  		string
  imgUrl { get; set; }


         [XmlElement("boundStatus")]
public  		int
  boundStatus { get; set; }


         [XmlElement("jdNum")]
public  		int
  jdNum { get; set; }


         [XmlElement("itemId")]
public  		long
  itemId { get; set; }


         [XmlElement("shareType")]
public  		int
  shareType { get; set; }


}
}
