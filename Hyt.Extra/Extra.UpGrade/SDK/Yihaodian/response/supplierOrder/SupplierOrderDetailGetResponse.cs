using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.SupplierOrder;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 查询订单详情
	/// </summary>
	public class SupplierOrderDetailGetResponse 
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

		/**商品详情 */
		[XmlElement("orderItemDetailInfoList")]
		public OrderItemDetailInfoList  OrderItemDetailInfoList{ get; set; }

	}
}
