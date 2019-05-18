using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Promotion;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询子品促销信息
	/// </summary>
	public class SubPromotionPriceGetResponse 
		: YhdResponse 
	{
		/**子品促销信息列表 */
		[XmlElement("productPromDetails")]
		public ProductPromDetailList  ProductPromDetails{ get; set; }

		/**成功记录数 */
		[XmlElement("totalCount")]
			public int?  TotalCount{ get; set; }

		/**失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

	}
}
