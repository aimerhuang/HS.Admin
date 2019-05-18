using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询商品状态
	/// </summary>
	public class UnionFullProductsGetResponse 
		: YhdResponse 
	{
		/**网盟全量判断是否下架接口返回的商品对象信息列表 */
		[XmlElement("union_full_products_outer_list")]
		public UnionFullProductsOuterList  Union_full_products_outer_list{ get; set; }

		/**符合条件的商品总数 */
		[XmlElement("total_results")]
			public int?  Total_results{ get; set; }

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
