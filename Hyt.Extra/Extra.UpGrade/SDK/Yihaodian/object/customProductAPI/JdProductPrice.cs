using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.CustomProductAPI
{
	/// <summary>
	/// JD商品价格信息
	/// </summary>
	[Serializable]
	public class JdProductPrice 
	{
		/**产品ID */
		[XmlElement("productId")]
			public string  ProductId{ get; set; }

		/**JD商品ID */
		[XmlElement("outerId")]
			public string  OuterId{ get; set; }

		/**基准价 */
		[XmlElement("nonMemberPrice")]
			public double?  NonMemberPrice{ get; set; }

		/**市场价 */
		[XmlElement("productListPrice")]
			public double?  ProductListPrice{ get; set; }

	}
}
