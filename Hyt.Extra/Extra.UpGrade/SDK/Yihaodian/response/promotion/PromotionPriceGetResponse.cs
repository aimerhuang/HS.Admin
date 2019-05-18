using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Promotion;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询价格促销列表
	/// </summary>
	public class PromotionPriceGetResponse 
		: YhdResponse 
	{
		/**成功记录数 */
		[XmlElement("totalCount")]
			public int?  TotalCount{ get; set; }

		/**失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表，存在错误信息时返回 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

		/**促销信息列表 */
		[XmlElement("productPromDetails")]
		public ProductPromDetailList  ProductPromDetails{ get; set; }

	}
}
