using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Promotion;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查找单个满就换购详情促销
	/// </summary>
	public class PromotionFullchangeSingleGetResponse 
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

		/**返回的结果 */
		[XmlElement("giftPromDetail")]
		public GiftPromDetail  GiftPromDetail{ get; set; }

	}
}
