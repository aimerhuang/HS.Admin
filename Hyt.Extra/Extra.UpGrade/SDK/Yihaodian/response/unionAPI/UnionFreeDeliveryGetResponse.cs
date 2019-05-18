using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 包邮商品信息接口 
	/// </summary>
	public class UnionFreeDeliveryGetResponse 
		: YhdResponse 
	{
		/**符合查询条件的总条数 */
		[XmlElement("total_result")]
			public int?  Total_result{ get; set; }

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

		/**包邮商品对象 */
		[XmlElement("union_api_get_product_by_price_outer_list")]
		public UnionApiGetFreeDeliveryProductOuterList  Union_api_get_product_by_price_outer_list{ get; set; }

	}
}
