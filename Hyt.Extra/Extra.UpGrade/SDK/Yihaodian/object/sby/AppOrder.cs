using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Sby
{
	/// <summary>
	/// SBY订单信息
	/// </summary>
	[Serializable]
	public class AppOrder 
	{
		/**主键 */
		[XmlElement("id")]
			public int?  Id{ get; set; }

		/**应用id */
		[XmlElement("appId")]
			public int?  AppId{ get; set; }

		/**版本id */
		[XmlElement("appVerId")]
			public int?  AppVerId{ get; set; }

		/**价格id */
		[XmlElement("appPriceId")]
			public int?  AppPriceId{ get; set; }

		/**订单号 */
		[XmlElement("orderNo")]
			public string  OrderNo{ get; set; }

		/**订单类型（1：新购2：升级3：续费） */
		[XmlElement("orderType")]
			public int?  OrderType{ get; set; }

		/**SBY用户id：App提供者 */
		[XmlElement("appSbyUserId")]
			public int?  AppSbyUserId{ get; set; }

		/**SBY用户id：购买者 */
		[XmlElement("buySbyUserId")]
			public int?  BuySbyUserId{ get; set; }

		/**实际付款价格 */
		[XmlElement("buyPrice")]
			public double?  BuyPrice{ get; set; }

		/**购买信息 */
		[XmlElement("buyPriceNote")]
			public string  BuyPriceNote{ get; set; }

		/**下单时间 */
		[XmlElement("buyDatetime")]
			public string  BuyDatetime{ get; set; }

		/**是否付款（0：未付款1：付款） */
		[XmlElement("isPay")]
			public int?  IsPay{ get; set; }

		/**付款时间 */
		[XmlElement("payDatetime")]
			public string  PayDatetime{ get; set; }

		/**1：计时2：计数 */
		[XmlElement("costType")]
			public int?  CostType{ get; set; }

		/**版本实际价格 */
		[XmlElement("appPrice")]
			public double?  AppPrice{ get; set; }

		/**类型为1时记录次数100次1000次/类型为2时记录天数30天60天360天等 */
		[XmlElement("appPriceNote")]
			public string  AppPriceNote{ get; set; }

		/**放数值 */
		[XmlElement("appCalVal")]
			public int?  AppCalVal{ get; set; }

		/**0：天、1：周、2：月、3：季、4：年、5：个、6：次 */
		[XmlElement("appCalUnit")]
			public int?  AppCalUnit{ get; set; }

		/**是否删除标记（0：未删除，1：删除） */
		[XmlElement("isDeleted")]
			public int?  IsDeleted{ get; set; }

		/**创建时间 */
		[XmlElement("createTime")]
			public string  CreateTime{ get; set; }

		/**最后更新时间 */
		[XmlElement("updateTime")]
			public string  UpdateTime{ get; set; }

		/**创建者id */
		[XmlElement("createId")]
			public int?  CreateId{ get; set; }

		/**更新者id */
		[XmlElement("updateId")]
			public int?  UpdateId{ get; set; }

	}
}
