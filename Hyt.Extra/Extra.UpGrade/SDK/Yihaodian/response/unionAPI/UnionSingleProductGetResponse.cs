using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询单品接口商品信息
	/// </summary>
	public class UnionSingleProductGetResponse 
		: YhdResponse 
	{
		/**单品接口返回的商品对象信息列表 */
		[XmlElement("single_product_info_outer_list")]
		public SingleProductInfoOuterList  Single_product_info_outer_list{ get; set; }

		/**符合条件的商品总数 */
		[XmlElement("total_results")]
			public int?  Total_results{ get; set; }

	}
}
