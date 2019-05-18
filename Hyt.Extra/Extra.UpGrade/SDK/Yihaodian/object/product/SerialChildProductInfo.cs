using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	/// <summary>
	/// 系列子产品新增信息
	/// </summary>
	[Serializable]
	public class SerialChildProductInfo 
	{
		/**外部产品编码(不超过30个字符) */
		[XmlElement("outerId")]
			public string  OuterId{ get; set; }

		/**是否是主产品（商品详情页默认显示的商品，最多只允许设置一个主产品） */
		[XmlElement("isMainProduct")]
			public int?  IsMainProduct{ get; set; }

		/**自定义颜色名称 */
		[XmlElement("colorName")]
			public string  ColorName{ get; set; }

		/**颜色属性项ItemId */
		[XmlElement("colorAttributeItemId")]
			public long?  ColorAttributeItemId{ get; set; }

		/**尺码属性项ItemId */
		[XmlElement("sizeAttributeItemId")]
			public long?  SizeAttributeItemId{ get; set; }

		/**市场价(最多两位小数) */
		[XmlElement("productMarketPrice")]
			public double?  ProductMarketPrice{ get; set; }

		/**销售价(不能大于市场价,最多两位小数) */
		[XmlElement("productSalePrice")]
			public double?  ProductSalePrice{ get; set; }

		/**库存(大于或等于0) */
		[XmlElement("virtualStockNum")]
			public long?  VirtualStockNum{ get; set; }

		/**节能补贴金额(不能大于市场价,最多两位小数,50~400) */
		[XmlElement("subsidyAmount")]
			public double?  SubsidyAmount{ get; set; }

		/**子品属性串。格式为pid1_vid1_aliasName1;pid2_vid2_aliasName2。其中pid代表系列属性id；vid代表属性的值id；aliasName代表属性值别名（pid、vid与aliasName之间用下划线_分隔；两个属性之间用分号;分隔）。若没有别名，请用标准属性值代替（不能不填）。别名中不能包含冒号:和分号;和逗号， */
		[XmlElement("properties")]
			public string  Properties{ get; set; }

	}
}
