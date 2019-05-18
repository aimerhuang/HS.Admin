using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Order
{
	/// <summary>
	/// 合约机订单信息
	/// </summary>
	[Serializable]
	public class ContractPhoneInfo 
	{
		/**机主姓名 */
		[XmlElement("hostName")]
			public string  HostName{ get; set; }

		/**证件类型。1：身份证 */
		[XmlElement("credentialsType")]
			public int?  CredentialsType{ get; set; }

		/**证件号码 */
		[XmlElement("credentialsNum")]
			public string  CredentialsNum{ get; set; }

		/**证件地址 */
		[XmlElement("credentialsAddress")]
			public string  CredentialsAddress{ get; set; }

		/**有效期 */
		[XmlElement("validDate")]
			public string  ValidDate{ get; set; }

		/**联系地址 */
		[XmlElement("contactAddress")]
			public string  ContactAddress{ get; set; }

		/**邮编 */
		[XmlElement("postCode")]
			public string  PostCode{ get; set; }

		/**联系电话 */
		[XmlElement("contactPhone")]
			public string  ContactPhone{ get; set; }

		/**手机号(客户选的号) */
		[XmlElement("mobilePhone")]
			public string  MobilePhone{ get; set; }

		/**合约机状态 */
		[XmlElement("informationStatus")]
			public int?  InformationStatus{ get; set; }

		/**备注 */
		[XmlElement("remark")]
			public string  Remark{ get; set; }

		/**确认人 */
		[XmlElement("confirmOperatorId")]
			public long?  ConfirmOperatorId{ get; set; }

		/**取消人 */
		[XmlElement("cancelOperatorId")]
			public long?  CancelOperatorId{ get; set; }

		/**创建时间 */
		[XmlElement("createTime")]
			public string  CreateTime{ get; set; }

		/**确认日期 */
		[XmlElement("confirmTime")]
			public string  ConfirmTime{ get; set; }

		/**取消日期 */
		[XmlElement("cancelTime")]
			public string  CancelTime{ get; set; }

		/**订单编码 */
		[XmlElement("orderCode")]
			public string  OrderCode{ get; set; }

		/**当月资费处理方式 */
		[XmlElement("currentMonthCostType")]
			public int?  CurrentMonthCostType{ get; set; }

		/**套餐名称 */
		[XmlElement("mealName")]
			public string  MealName{ get; set; }

		/**预存款（所属0元购机） */
		[XmlElement("beforehandMoney")]
			public double?  BeforehandMoney{ get; set; }

		/**每月返还金额（所属0元购机) */
		[XmlElement("monthlyRefundMoney")]
			public double?  MonthlyRefundMoney{ get; set; }

		/**手机款（所属0元购机) */
		[XmlElement("phoneMoney")]
			public double?  PhoneMoney{ get; set; }

		/**合约期赠费总金额（所属购机送话费） */
		[XmlElement("contractGiveMoney")]
			public double?  ContractGiveMoney{ get; set; }

		/**每月赠费金额（所属购机送话费） */
		[XmlElement("monthlyGiveMoney")]
			public double?  MonthlyGiveMoney{ get; set; }

		/**套餐月费（或合约套餐） */
		[XmlElement("monthRent")]
			public string  MonthRent{ get; set; }

		/**签约在网时长 */
		[XmlElement("contractMonthNum")]
			public string  ContractMonthNum{ get; set; }

		/**入网返还金额 */
		[XmlElement("enterReturnMoney")]
			public double?  EnterReturnMoney{ get; set; }

		/**用户身份证照片URL(正面) */
		[XmlElement("identifyFrontPictureUrl")]
			public string  IdentifyFrontPictureUrl{ get; set; }

		/**用户身份证照片URL(反面) */
		[XmlElement("identifyBackPictureUrl")]
			public string  IdentifyBackPictureUrl{ get; set; }

		/**商家id */
		[XmlElement("merchantId")]
			public long?  MerchantId{ get; set; }

		/**合约所属地区 */
		[XmlElement("provinceCityName")]
			public string  ProvinceCityName{ get; set; }

		/**合约名称 */
		[XmlElement("contractName")]
			public string  ContractName{ get; set; }

		/**移动运营商1:联通，2：移动，3：电信 */
		[XmlElement("mobileOperator")]
			public long?  MobileOperator{ get; set; }

		/**合约类型：0：0元购机 1：购机送话费 */
		[XmlElement("contractType")]
			public int?  ContractType{ get; set; }

	}
}
