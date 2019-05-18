using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	/// <summary>
	/// 商品基本信息
	/// </summary>
	[Serializable]
	public class ProductItem 
	{
		/**商品ID */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**商品中文名称 */
		[XmlElement("title")]
			public string  Title{ get; set; }

		/**商品价格 */
		[XmlElement("price")]
			public double?  Price{ get; set; }

		/**7天内交易量 */
		[XmlElement("volume")]
			public long?  Volume{ get; set; }

		/**图片url */
		[XmlElement("picUrl")]
			public string  PicUrl{ get; set; }

		/**产品url地址(仅限手机端) */
		[XmlElement("url")]
			public string  Url{ get; set; }

	}
}
