using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Product
{
	/// <summary>
	/// 产品系列属性信息
	/// </summary>
	[Serializable]
	public class ProdSerialAttributeInfo 
	{
		/**系列产品ID */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**系列产品外部ID */
		[XmlElement("outerId")]
			public string  OuterId{ get; set; }

		/**系列子品属性信息列表 */
		[XmlElement("serialProdAttributeInfoList")]
		public SerialProdAttributeInfoList  SerialProdAttributeInfoList{ get; set; }

		/**自定义颜色名称 注：输入参数为虚品时，只返回主品的自定义颜色名称 */
		[XmlElement("customColorName")]
			public string  CustomColorName{ get; set; }

		/**自定义尺码名称 注：输入参数为虚品时，只返回主品的自定义尺码名称 */
		[XmlElement("customSizeName")]
			public string  CustomSizeName{ get; set; }

		/**系列产品属性。多个属性串之间用分号分隔。格式：pid1:vid1:aliasName1;pid2:vid2:aliasName2 如果新增时没有给出别名，则aliasName将用null代替。 */
		[XmlElement("customProperties")]
			public string  CustomProperties{ get; set; }

	}
}
