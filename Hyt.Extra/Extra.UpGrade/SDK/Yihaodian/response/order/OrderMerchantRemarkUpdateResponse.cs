using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 更新单个订单的卖家备注信息
	/// </summary>
	public class OrderMerchantRemarkUpdateResponse 
		: YhdResponse 
	{
		/**更新成功记录数 */
		[XmlElement("updateCount")]
			public int?  UpdateCount{ get; set; }

		/**更新失败记录数 */
		[XmlElement("errorCount")]
			public int?  ErrorCount{ get; set; }

		/**错误信息列表 */
		[XmlElement("errInfoList")]
		public ErrDetailInfoList  ErrInfoList{ get; set; }

	}
}
