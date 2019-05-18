using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierOrder
{
	/// <summary>
	/// PO单对象
	/// </summary>
	[Serializable]
	public class Po 
	{
		/**采购单Id */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**采购单编码 */
		[XmlElement("poCode")]
			public string  PoCode{ get; set; }

		/**供应商id */
		[XmlElement("supplierId")]
			public long?  SupplierId{ get; set; }

		/**PO状态： 0待批准，1批准，2待收货，3部分收货，4拒绝收货，5完成，6 终止，7等待取消，8退货完成，9作废 */
		[XmlElement("poStatus")]
			public int?  PoStatus{ get; set; }

		/**总金额 */
		[XmlElement("poAmount")]
			public double?  PoAmount{ get; set; }

		/**下单日期 */
		[XmlElement("poOrderDate")]
			public string  PoOrderDate{ get; set; }

		/**约定交货日期 */
		[XmlElement("forecastDeliveryDate")]
			public string  ForecastDeliveryDate{ get; set; }

		/**0:采购1：退货 */
		[XmlElement("poType")]
			public int?  PoType{ get; set; }

		/**总数量 */
		[XmlElement("totalNum")]
			public long?  TotalNum{ get; set; }

		/**采购模式0:正常po,1自动po */
		[XmlElement("purchaseMode")]
			public int?  PurchaseMode{ get; set; }

		/**待收发货时间 */
		[XmlElement("dsfhDate")]
			public string  DsfhDate{ get; set; }

		/**PO单有效期  */
		[XmlElement("validDate")]
			public string  ValidDate{ get; set; }

		/**效期类型 */
		[XmlElement("periodValid")]
			public int?  PeriodValid{ get; set; }

		/**送货方式(只限订单，退货的退货方式在退货单查询中) */
		[XmlElement("deliveryMethod")]
			public int?  DeliveryMethod{ get; set; }

		/**收货仓库 */
		[XmlElement("wareHouseName")]
			public string  WareHouseName{ get; set; }

		/**采购单详情列表 */
		[XmlElement("poItemList")]
		public PoItemList  PoItemList{ get; set; }

		/**实际交货日期 */
		[XmlElement("actualDeliveryDate")]
			public string  ActualDeliveryDate{ get; set; }

	}
}
