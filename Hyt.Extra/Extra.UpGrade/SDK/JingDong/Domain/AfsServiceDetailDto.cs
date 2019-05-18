using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AfsServiceDetailDto : JdObject{


         [XmlElement("customer_order_sku_id_pop")]
public  		string
  customerOrderSkuIdPop { get; set; }


         [XmlElement("customer_order_sku_id_outer")]
public  		string
  customerOrderSkuIdOuter { get; set; }


         [XmlElement("customer_order_sku_name")]
public  		string
  customerOrderSkuName { get; set; }


         [XmlElement("customer_order_apply_num")]
public  		int
  customerOrderApplyNum { get; set; }


         [XmlElement("customer_order_verify_num")]
public  		int
  customerOrderVerifyNum { get; set; }


         [XmlElement("customer_order_finish_remark")]
public  		string
  customerOrderFinishRemark { get; set; }


}
}
