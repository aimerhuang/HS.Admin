using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Sby
{
	/// <summary>
	/// 应用信息
	/// </summary>
	[Serializable]
	public class FuwuApp 
	{
		/**主key */
		[XmlElement("id")]
			public int?  Id{ get; set; }

		/**购买者用户id */
		[XmlElement("buySbyUserId")]
			public int?  BuySbyUserId{ get; set; }

		/**AppId */
		[XmlElement("appId")]
			public int?  AppId{ get; set; }

		/**APP编码 */
		[XmlElement("appCode")]
			public string  AppCode{ get; set; }

		/**APP版本ID */
		[XmlElement("appVerId")]
			public int?  AppVerId{ get; set; }

		/**价格id */
		[XmlElement("appPriceId")]
			public int?  AppPriceId{ get; set; }

		/**开始计费日期 */
		[XmlElement("startDate")]
			public string  StartDate{ get; set; }

		/**到期日期 */
		[XmlElement("expireDate")]
			public string  ExpireDate{ get; set; }

		/**总次数 */
		[XmlElement("totalCount")]
			public int?  TotalCount{ get; set; }

		/**可用次数 */
		[XmlElement("validCount")]
			public int?  ValidCount{ get; set; }

		/**是否立开始计费1为立即开始计费 */
		[XmlElement("isBilling")]
			public int?  IsBilling{ get; set; }

		/**收费类型1为计时2为计次 */
		[XmlElement("costType")]
			public int?  CostType{ get; set; }

		/**收费价格 */
		[XmlElement("buyPrice")]
			public double?  BuyPrice{ get; set; }

		/**收费价格说明 */
		[XmlElement("buyPriceNote")]
			public string  BuyPriceNote{ get; set; }

		/**收费计算值 */
		[XmlElement("appCalVal")]
			public int?  AppCalVal{ get; set; }

		/**收费计算单位 */
		[XmlElement("appCalUnit")]
			public string  AppCalUnit{ get; set; }

		/**是否删除1是已删除0是未删除 */
		[XmlElement("isDeleted")]
			public int?  IsDeleted{ get; set; }

		/**创建时间 */
		[XmlElement("createTime")]
			public string  CreateTime{ get; set; }

		/**更新时间 */
		[XmlElement("updateTime")]
			public string  UpdateTime{ get; set; }

		/**创建者id */
		[XmlElement("createId")]
			public int?  CreateId{ get; set; }

		/**更新者 */
		[XmlElement("updateId")]
			public int?  UpdateId{ get; set; }

	}
}
