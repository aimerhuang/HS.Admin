using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class StockBillSamInfo : JdObject{


         [XmlElement("id")]
public  		long
  id { get; set; }


         [XmlElement("sam_bill_id")]
public  		long
  samBillId { get; set; }


         [XmlElement("club_id")]
public  		long
  clubId { get; set; }


         [XmlElement("goods_num_apply")]
public  		long
  goodsNumApply { get; set; }


         [XmlElement("goods_money_apply")]
public  		double
  goodsMoneyApply { get; set; }


         [XmlElement("time_apply")]
public  		DateTime
  timeApply { get; set; }


         [XmlElement("goods_num_actual")]
public  		long
  goodsNumActual { get; set; }


         [XmlElement("goods_money_actual")]
public  		double
  goodsMoneyActual { get; set; }


         [XmlElement("time_actual")]
public  		DateTime
  timeActual { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("detail_list")]
public  		List<string>
  detailList { get; set; }


}
}
