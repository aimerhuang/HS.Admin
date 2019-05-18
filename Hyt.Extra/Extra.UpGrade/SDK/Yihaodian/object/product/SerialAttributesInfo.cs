using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	/// <summary>
	/// 类别系列属性信息
	/// </summary>
	[Serializable]
	public class SerialAttributesInfo 
	{
		/**颜色属性项ID(从获取类别系列属性(yhd.category.serial.attribute.get)接口中获取) */
		[XmlElement("colorItemId")]
			public long?  ColorItemId{ get; set; }

		/**自定义颜色名称(若colorItemId相同，则以第一个名称为主) */
		[XmlElement("colorName")]
			public string  ColorName{ get; set; }

		/**尺寸属性项ID(从获取类别系列属性yhd.category.serial.attribute.get接口中获取) */
		[XmlElement("sizeItemId")]
			public long?  SizeItemId{ get; set; }

		/**市场价 */
		[XmlElement("productMarketPrice")]
			public double?  ProductMarketPrice{ get; set; }

		/**销售价 */
		[XmlElement("productSalePrice")]
			public double?  ProductSalePrice{ get; set; }

		/**库存 */
		[XmlElement("virtualStockNum")]
			public long?  VirtualStockNum{ get; set; }

		/**外部产品编码 */
		[XmlElement("outerId")]
			public string  OuterId{ get; set; }

		/**是否是主产品(1:是,0:否) */
		[XmlElement("isMainProduct")]
			public int?  IsMainProduct{ get; set; }

		/**节能补贴金额(不能大于市场价,最多两位小数,50~400) */
		[XmlElement("subsidyAmount")]
			public double?  SubsidyAmount{ get; set; }

		/**子品属性串。格式为pid1_vid1_aliasName1;pid2_vid2_aliasName2。其中pid代表系列属性id；vid代表属性的值id；aliasName代表属性值别名（pid、vid与aliasName之间用下划线_分隔；两个属性之间用分号;分隔）。若不需要别名，请用字符串null代替。如21146_21162_null;21147_21163_null。别名中不能包含冒号:和分号;和逗号， */
		[XmlElement("properties")]
			public string  Properties{ get; set; }

	}
}
