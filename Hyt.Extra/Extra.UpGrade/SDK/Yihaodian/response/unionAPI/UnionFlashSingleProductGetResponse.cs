using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询名品商品详情信息
	/// </summary>
	public class UnionFlashSingleProductGetResponse 
		: YhdResponse 
	{
		/**名品商品集合对象 */
		[XmlElement("union_flash_product_info_outer")]
		public UnionFlashProductInfoOuterList  Union_flash_product_info_outer{ get; set; }

	}
}
