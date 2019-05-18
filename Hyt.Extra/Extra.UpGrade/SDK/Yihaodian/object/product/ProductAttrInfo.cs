using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	/// <summary>
	/// 产品属性信息
	/// </summary>
	[Serializable]
	public class ProductAttrInfo 
	{
		/**产品ID */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**外部产品ID */
		[XmlElement("outerId")]
			public string  OuterId{ get; set; }

		/**单个产品属性信息列表 */
		[XmlElement("productAttributesInfoList")]
		public ProductAttributesInfoList  ProductAttributesInfoList{ get; set; }

	}
}
