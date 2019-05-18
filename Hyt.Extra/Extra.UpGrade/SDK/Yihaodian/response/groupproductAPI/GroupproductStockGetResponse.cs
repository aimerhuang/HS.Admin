using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.GroupproductAPI;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 获取集团商家产品库存信息
	/// </summary>
	public class GroupproductStockGetResponse 
		: YhdResponse 
	{
		/**库存产品列表 */
		[XmlElement("pmStock")]
		public PmStockList  PmStock{ get; set; }

		/**成功记录数 */
		[XmlElement("totalCount")]
			public int?  TotalCount{ get; set; }

		/**失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
