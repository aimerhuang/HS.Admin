using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询名品类目下的卖场信息
	/// </summary>
	public class UnionFlashBuysGetResponse 
		: YhdResponse 
	{
		/**名品卖场对象集合 */
		[XmlElement("union_flash_buy_info_list")]
		public UnionFlashBuyInfoOuterList  Union_flash_buy_info_list{ get; set; }

		/**符合条件的商品总数 */
		[XmlElement("total_size")]
			public int?  Total_size{ get; set; }

	}
}
