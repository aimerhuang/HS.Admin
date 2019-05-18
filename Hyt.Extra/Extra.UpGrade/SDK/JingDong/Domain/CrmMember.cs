using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CrmMember : JdObject{


         [XmlElement("customer_pin")]
public  		string
  customerPin { get; set; }


         [XmlElement("grade")]
public  		string
  grade { get; set; }


         [XmlElement("trade_count")]
public  		string
  tradeCount { get; set; }


         [XmlElement("trade_amount")]
public  		string
  tradeAmount { get; set; }


         [XmlElement("close_trade_count")]
public  		string
  closeTradeCount { get; set; }


         [XmlElement("close_trade_amount")]
public  		string
  closeTradeAmount { get; set; }


         [XmlElement("item_num")]
public  		string
  itemNum { get; set; }


         [XmlElement("avg_price")]
public  		string
  avgPrice { get; set; }


         [XmlElement("last_trade_time")]
public  		DateTime
  lastTradeTime { get; set; }


}
}
