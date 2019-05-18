using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Trade;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询卖家已卖出的交易数据（根据创建时间）（兼容淘宝）
	/// </summary>
	public class TradesSoldGetResponse 
		: YhdResponse 
	{
		/**搜索到的交易信息总数 */
		[XmlElement("total_results")]
			public long?  Total_results{ get; set; }

		/**搜索到的交易信息列表，返回的Trade和Order中包含的具体信息为入参fields请求的字段信息 */
		[XmlElement("trades")]
		public TradeList  Trades{ get; set; }

		/**是否存在下一页 */
		[XmlElement("has_next")]
			public bool  Has_next{ get; set; }

		/**错误码 */
		[XmlElement("error_code")]
			public string  Error_code{ get; set; }

		/**子错误描述 */
		[XmlElement("sub_msg")]
			public string  Sub_msg{ get; set; }

		/**错误描述 */
		[XmlElement("msg")]
			public string  Msg{ get; set; }

		/**子错误码 */
		[XmlElement("sub_code")]
			public string  Sub_code{ get; set; }

	}
}
