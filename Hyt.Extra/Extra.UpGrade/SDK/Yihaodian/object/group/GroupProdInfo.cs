using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Group
{
	/// <summary>
	/// 团购报名信息
	/// </summary>
	[Serializable]
	public class GroupProdInfo 
	{
		/**1号店产品ID */
		[XmlElement("productId")]
			public long?  ProductId{ get; set; }

		/**团购ID */
		[XmlElement("groupId")]
			public long?  GroupId{ get; set; }

		/**团购产品名称 */
		[XmlElement("groupCname")]
			public string  GroupCname{ get; set; }

		/**团购价格 */
		[XmlElement("groupPrice")]
			public double?  GroupPrice{ get; set; }

		/**团购购买数量下限 */
		[XmlElement("minStockNum")]
			public int?  MinStockNum{ get; set; }

		/**团购购买数量上限 */
		[XmlElement("maxStockNum")]
			public int?  MaxStockNum{ get; set; }

		/**每人购买数量下限 */
		[XmlElement("minGroupNum")]
			public int?  MinGroupNum{ get; set; }

		/**每人购买数量上限 */
		[XmlElement("maxGroupNum")]
			public int?  MaxGroupNum{ get; set; }

		/**团购预告开始时间 */
		[XmlElement("prepareTime")]
			public string  PrepareTime{ get; set; }

		/**团购开始时间 */
		[XmlElement("startTime")]
			public string  StartTime{ get; set; }

		/**团购结束时间 */
		[XmlElement("endTime")]
			public string  EndTime{ get; set; }

		/**活动地区(销售地区,逗号分隔) */
		[XmlElement("saleAreaId")]
			public string  SaleAreaId{ get; set; }

		/**团购状态(WAIT_VERIFYING:待审核;VERIFY_REFUESD:审核拒绝;VERIFY_PASSED:审核通过;GROUPON:团购中; GROUPON_SUCCESS:团购中-成功;G */
		[XmlElement("groupStatus")]
			public string  GroupStatus{ get; set; }

		/**团购销售数量 */
		[XmlElement("groupSaleNum")]
			public int?  GroupSaleNum{ get; set; }

		/**团购短名称 */
		[XmlElement("shortName")]
			public string  ShortName{ get; set; }

		/**是否生活团，0：否 1：是 */
		[XmlElement("virtualType")]
			public int?  VirtualType{ get; set; }

		/**是否不限量，0：限量 1：不限量，默认为0 限量：根据库存和设置的团购上限来同时判断 不限量：默认最大支持10w，只根据库存来判断 */
		[XmlElement("unlimitedFlag")]
			public int?  UnlimitedFlag{ get; set; }

		/**系列子品信息（子品ID:销售上限;子品ID:销售上限） */
		[XmlElement("subProductList")]
			public string  SubProductList{ get; set; }

		/**审核失败原因 */
		[XmlElement("auditReason")]
			public string  AuditReason{ get; set; }

		/**团购类别id */
		[XmlElement("groupCategoryId")]
			public long?  GroupCategoryId{ get; set; }

	}
}
