using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询金牌秒杀列表信息
	/// </summary>
	public class UnionGrouponHourbuyGetResponse 
		: YhdResponse 
	{
		/**金牌秒杀团购信息 */
		[XmlElement("union_api_groupon_hourbuy_info_outer_list")]
		public UnionApiGrouponHourbuyInfoOuterList  Union_api_groupon_hourbuy_info_outer_list{ get; set; }

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

		/**总数 */
		[XmlElement("total_count")]
			public int?  Total_count{ get; set; }

	}
}
