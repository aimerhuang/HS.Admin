using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	/// <summary>
	/// 系列子品属性信息
	/// </summary>
	[Serializable]
	public class SerialProdAttributeInfo 
	{
		/**系列子品ID */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**系列子品外部ID */
		[XmlElement("outerId")]
			public string  OuterId{ get; set; }

		/**系列属性信息 */
		[XmlElement("serialAttributeInfoList")]
		public SerialAttributeInfoList  SerialAttributeInfoList{ get; set; }

	}
}
