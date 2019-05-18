using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 出库/妥投/拒收订单接口
	/// </summary>
	public class JdDealDeliveryorderResponse 
		: YhdResponse 
	{
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
