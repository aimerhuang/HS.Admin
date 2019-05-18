using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class MemberDynmModel : JdObject{


         [XmlElement("model_id")]
public  		string
  modelId { get; set; }


         [XmlElement("model_name")]
public  		string
  modelName { get; set; }


         [XmlElement("min_last_trade_time")]
public  		DateTime
  minLastTradeTime { get; set; }


         [XmlElement("max_last_trade_time")]
public  		DateTime
  maxLastTradeTime { get; set; }


         [XmlElement("avg_price")]
public  		string
  avgPrice { get; set; }


         [XmlElement("grade")]
public  		string
  grade { get; set; }


         [XmlElement("min_trade_count")]
public  		int
  minTradeCount { get; set; }


         [XmlElement("max_trade_count")]
public  		int
  maxTradeCount { get; set; }


         [XmlElement("min_trade_amount")]
public  		string
  minTradeAmount { get; set; }


}
}
