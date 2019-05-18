using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	/// <summary>
	/// 套餐子产品
	/// </summary>
	[Serializable]
	public class ComChildProd 
	{
		/**商家产品编码 */
		[XmlElement("productCode")]
			public string  ProductCode{ get; set; }

		/**商家产品中文名称 */
		[XmlElement("productCname")]
			public string  ProductCname{ get; set; }

		/**产品ID */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**产品条形码 */
		[XmlElement("ean13")]
			public string  Ean13{ get; set; }

		/**产品类目ID */
		[XmlElement("categoryId")]
			public long?  CategoryId{ get; set; }

		/**上下架状态0：下架，1：上架 */
		[XmlElement("canSale")]
			public int?  CanSale{ get; set; }

		/**产品库存状态 */
		[XmlElement("stockStatus")]
			public int?  StockStatus{ get; set; }

		/**外部产品ID */
		[XmlElement("outerId")]
			public string  OuterId{ get; set; }

		/**是否可见(强制上/下架),1是0否 */
		[XmlElement("canShow")]
			public int?  CanShow{ get; set; }

		/**产品的数量 */
		[XmlElement("productNum")]
			public int?  ProductNum{ get; set; }

		/**是否为主打产品（1：是、0：否） */
		[XmlElement("isMainProduct")]
			public int?  IsMainProduct{ get; set; }

	}
}
