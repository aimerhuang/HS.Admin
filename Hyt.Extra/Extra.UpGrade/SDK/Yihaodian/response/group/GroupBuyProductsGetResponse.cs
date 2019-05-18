using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Group;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 团购报名查询
	/// </summary>
	public class GroupBuyProductsGetResponse 
		: YhdResponse 
	{
		/**团购报名信息 */
		[XmlElement("groupProdInfoList")]
		public GroupProdInfoList  GroupProdInfoList{ get; set; }

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
