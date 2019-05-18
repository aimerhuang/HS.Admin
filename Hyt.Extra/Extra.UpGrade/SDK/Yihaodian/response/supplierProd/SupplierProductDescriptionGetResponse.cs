using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.SupplierProd;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 批量查询供应商产品文描信息
	/// </summary>
	public class SupplierProductDescriptionGetResponse 
		: YhdResponse 
	{
		/**供应商产品文描信息列表 */
		[XmlElement("productDescInfoList")]
		public ProductDescInfoList  ProductDescInfoList{ get; set; }

		/**查询成功记录数 */
		[XmlElement("totalCount")]
			public int?  TotalCount{ get; set; }

		/**查询失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
