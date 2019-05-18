using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	/// <summary>
	/// 产品文描信息
	/// </summary>
	[Serializable]
	public class ProductDescInfo 
	{
		/**产品ID */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**外部产品ID */
		[XmlElement("outerId")]
			public string  OuterId{ get; set; }

		/**类别tabID */
		[XmlElement("categoryTabId")]
			public long?  CategoryTabId{ get; set; }

		/**操作人id */
		[XmlElement("creator")]
			public long?  Creator{ get; set; }

		/**创建时间 */
		[XmlElement("createTime")]
			public string  CreateTime{ get; set; }

		/**修改人id */
		[XmlElement("modifier")]
			public long?  Modifier{ get; set; }

		/**更新时间 */
		[XmlElement("modiTime")]
			public string  ModiTime{ get; set; }

		/**tab内容 */
		[XmlElement("tabDetail")]
			public string  TabDetail{ get; set; }

		/**是否读取产品属性 */
		[XmlElement("isReadPro")]
			public int?  IsReadPro{ get; set; }

	}
}
