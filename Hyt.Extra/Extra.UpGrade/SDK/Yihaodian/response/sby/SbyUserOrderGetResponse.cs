using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Sby;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询用户历史app订单
	/// </summary>
	public class SbyUserOrderGetResponse 
		: YhdResponse 
	{
		/**查询成功 */
		[XmlElement("totalCount")]
			public int?  TotalCount{ get; set; }

		/**app订单信息 */
		[XmlElement("appOrderList")]
		public AppOrderList  AppOrderList{ get; set; }

		/**查询失败 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
