using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Product;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 根据关键字或类目ID查询商品
	/// </summary>
	public class ProductSearchBykeyResponse 
		: YhdResponse 
	{
		/**查询成功记录数 */
		[XmlElement("totalCount")]
			public int?  TotalCount{ get; set; }

		/**查询失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

		/**商品信息列表 */
		[XmlElement("productItemList")]
		public ProductItemList  ProductItemList{ get; set; }

	}
}
