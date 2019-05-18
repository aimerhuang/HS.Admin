using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AfsServiceSendResponse : JdObject{


         [XmlElement("afs_no")]
public  		string
  afsNo { get; set; }


         [XmlElement("pic_war_cod")]
public  		string
  picWarCod { get; set; }


         [XmlElement("ord_no")]
public  		string
  ordNo { get; set; }


         [XmlElement("inv_no")]
public  		string
  invNo { get; set; }


         [XmlElement("buy_pri")]
public  		string
  buyPri { get; set; }


         [XmlElement("cus_n")]
public  		string
  cusN { get; set; }


         [XmlElement("cus_mp")]
public  		string
  cusMp { get; set; }


         [XmlElement("prov_no")]
public  		string
  provNo { get; set; }


         [XmlElement("prov_n")]
public  		string
  provN { get; set; }


         [XmlElement("cty_no")]
public  		string
  ctyNo { get; set; }


         [XmlElement("cty_n")]
public  		string
  ctyN { get; set; }


         [XmlElement("cnty_no")]
public  		string
  cntyNo { get; set; }


         [XmlElement("cnty_n")]
public  		string
  cntyN { get; set; }


         [XmlElement("tn_no")]
public  		string
  tnNo { get; set; }


         [XmlElement("tn_n")]
public  		string
  tnN { get; set; }


         [XmlElement("add")]
public  		string
  add { get; set; }


         [XmlElement("del_t")]
public  		string
  delT { get; set; }


         [XmlElement("has_inv")]
public  		int
  hasInv { get; set; }


         [XmlElement("aud_typ")]
public  		string
  audTyp { get; set; }


         [XmlElement("que_desc")]
public  		string
  queDesc { get; set; }


         [XmlElement("app_t")]
public  		string
  appT { get; set; }


         [XmlElement("cus_exp")]
public  		string
  cusExp { get; set; }


         [XmlElement("cus_exp_t")]
public  		string
  cusExpT { get; set; }


         [XmlElement("afs_sta")]
public  		int
  afsSta { get; set; }


         [XmlElement("app_num")]
public  		int
  appNum { get; set; }


         [XmlElement("bef_fin_rs")]
public  		string
  befFinRs { get; set; }


         [XmlElement("afs_det")]
public  		string
  afsDet { get; set; }


}
}
