using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询商家活动详情产品信息接口
	/// </summary>
	public class UnionBusinessActivityProductsGetResponse 
		: YhdResponse 
	{
		/**商家活动详情接口返回的商品对象信息列表 */
		[XmlElement("activity_detail_info_outer")]
		public ActivityDetailInfoOuterList  Activity_detail_info_outer{ get; set; }

		/**符合条件的商品总数 */
		[XmlElement("total_results")]
			public int?  Total_results{ get; set; }

	}
}
