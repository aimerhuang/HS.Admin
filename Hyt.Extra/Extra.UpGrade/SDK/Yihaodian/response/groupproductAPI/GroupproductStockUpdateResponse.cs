using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 更新集团商家单个产品库存信息
	/// </summary>
	public class GroupproductStockUpdateResponse 
		: YhdResponse 
	{
		/**实际更新的库存值 */
		[XmlElement("updateStockNum")]
			public long?  UpdateStockNum{ get; set; }

		/**区域商家id */
		[XmlElement("merchantId")]
			public long?  MerchantId{ get; set; }

		/**成功记录数 */
		[XmlElement("updateCount")]
			public int?  UpdateCount{ get; set; }

		/**失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
