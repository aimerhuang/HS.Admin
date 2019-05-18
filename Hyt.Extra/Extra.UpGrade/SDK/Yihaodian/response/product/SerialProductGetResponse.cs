using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Product;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 获取单个系列产品的子品信息
	/// </summary>
	public class SerialProductGetResponse 
		: YhdResponse 
	{
		/**查询成功记录数 */
		[XmlElement("totalCount")]
			public int?  TotalCount{ get; set; }

		/**查询失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**系列产品子产品信息 */
		[XmlElement("serialChildProdList")]
		public SerialChildProdList  SerialChildProdList{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
