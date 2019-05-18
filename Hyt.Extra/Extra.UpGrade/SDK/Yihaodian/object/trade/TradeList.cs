using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Trade
{
	[Serializable]
	public class TradeList 
	{	
		/// <summary>
		/// 搜索到的交易信息列表，返回的Trade和Order中包含的具体信息为入参fields请求的字段信息
		/// </summary>
		[XmlElement("trade")]
		public List<Trade>  Trade{ get; set; }
	}
}
