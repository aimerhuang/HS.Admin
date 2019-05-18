using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 获取抢购商品
	/// </summary>
	public class UnionQianggouProductGetResponse 
		: YhdResponse 
	{
		/**抢购商品list */
		[XmlElement("union_qiang_gou_product_info")]
		public UnionQiangGouProductInfo  Union_qiang_gou_product_info{ get; set; }

		/**调用接口状态码 */
		[XmlElement("status_code")]
			public int?  Status_code{ get; set; }

		/**调用接口状态信息 */
		[XmlElement("status_msg")]
			public string  Status_msg{ get; set; }

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
