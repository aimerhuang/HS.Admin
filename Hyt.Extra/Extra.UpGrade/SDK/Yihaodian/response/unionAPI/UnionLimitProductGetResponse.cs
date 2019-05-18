using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询限时购商品信息接口
	/// </summary>
	public class UnionLimitProductGetResponse 
		: YhdResponse 
	{
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

		/**限时购接口返回的商品对象信息列表 */
		[XmlElement("union_limit_products_outer")]
		public UnionLimitProductsOuterList  Union_limit_products_outer{ get; set; }

	}
}
