using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// 限时购接口返回的商品信息中比价商家的信息
	/// </summary>
	[Serializable]
	public class UnionLimitProductPriceCompare 
	{
		/**比价的商家 */
		[XmlElement("businesses_name")]
			public string  Businesses_name{ get; set; }

		/**比价商家价格 */
		[XmlElement("businesses_price")]
			public string  Businesses_price{ get; set; }

		/**比价商家链接 */
		[XmlElement("businesses_url")]
			public string  Businesses_url{ get; set; }

	}
}
