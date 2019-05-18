using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询名品卖场下所有的商品信息
	/// </summary>
	public class UnionFlashProductlistGetResponse 
		: YhdResponse 
	{
		/**名品对相集合 */
		[XmlElement("union_flash_product_info_outer")]
		public UnionFlashProductInfoOuterList  Union_flash_product_info_outer{ get; set; }

		/**符合条件的商品总数 */
		[XmlElement("total_results")]
			public int?  Total_results{ get; set; }

	}
}
