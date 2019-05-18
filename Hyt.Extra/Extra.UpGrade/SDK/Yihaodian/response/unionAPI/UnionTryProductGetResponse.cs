using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询试用中心商品信息
	/// </summary>
	public class UnionTryProductGetResponse 
		: YhdResponse 
	{
		/**试用中心商品数据信息列表 */
		[XmlElement("try_product_info_outer_list")]
		public TryProductInfoOuterList  Try_product_info_outer_list{ get; set; }

		/**符合条件的商品总数 */
		[XmlElement("total_results")]
			public int?  Total_results{ get; set; }

	}
}
