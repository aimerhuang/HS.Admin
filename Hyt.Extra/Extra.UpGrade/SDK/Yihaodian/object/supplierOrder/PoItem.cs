using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.SupplierOrder
{
	/// <summary>
	/// 采购单详情
	/// </summary>
	[Serializable]
	public class PoItem 
	{
		/**采购单Id */
		[XmlElement("poId")]
			public long?  PoId{ get; set; }

		/**进货价格 */
		[XmlElement("poItemInPrice")]
			public double?  PoItemInPrice{ get; set; }

		/**计量单位 */
		[XmlElement("poUnit")]
			public string  PoUnit{ get; set; }

		/**实际送货数量 */
		[XmlElement("actualDeliveryNum")]
			public double?  ActualDeliveryNum{ get; set; }

		/** 0:正品,1:坏品 */
		[XmlElement("returnStockType")]
			public int?  ReturnStockType{ get; set; }

		/**PO状态:PO状态： 待批准，批准，待收货，部分收货，拒绝收货，完成， 终止 */
		[XmlElement("poItemStatus")]
			public int?  PoItemStatus{ get; set; }

		/**商品编码 */
		[XmlElement("pmCode")]
			public string  PmCode{ get; set; }

		/**该产品在供应商处的类别 */
		[XmlElement("categoryInSupplier")]
			public string  CategoryInSupplier{ get; set; }

		/**税率 */
		[XmlElement("taxRate")]
			public double?  TaxRate{ get; set; }

		/** 1表示是新品，0或空表示不是新品 */
		[XmlElement("isNew")]
			public int?  IsNew{ get; set; }

		/**商品名称 */
		[XmlElement("productName")]
			public string  ProductName{ get; set; }

		/**条形码 */
		[XmlElement("ean13")]
			public string  Ean13{ get; set; }

		/**1号店商品类别 */
		[XmlElement("categoryName")]
			public string  CategoryName{ get; set; }

		/**市场价 */
		[XmlElement("productListPrice")]
			public double?  ProductListPrice{ get; set; }

		/**箱规 */
		[XmlElement("stdPackQty")]
			public int?  StdPackQty{ get; set; }

		/**订购数量 */
		[XmlElement("poItemNum")]
			public long?  PoItemNum{ get; set; }

		/**总金额 */
		[XmlElement("totalPrice")]
			public double?  TotalPrice{ get; set; }

		/**供应商确认供货数量 */
		[XmlElement("expectedAvailabilityNum")]
			public long?  ExpectedAvailabilityNum{ get; set; }

		/**供应商发货或拒绝原因 */
		[XmlElement("reasonForAvailability")]
			public int?  ReasonForAvailability{ get; set; }

		/**取消数量 */
		[XmlElement("cancelNum")]
			public long?  CancelNum{ get; set; }

		/**是否打印条码 */
		[XmlElement("isPrintBarcode")]
			public int?  IsPrintBarcode{ get; set; }

		/**PoItem的id */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**实际送货时间 */
		[XmlElement("actualDeliveryDate")]
			public string  ActualDeliveryDate{ get; set; }

		/**规格 */
		[XmlElement("format")]
			public string  Format{ get; set; }

	}
}
