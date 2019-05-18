using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Promotion
{
	/// <summary>
	/// 申请的活动详情
	/// </summary>
	[Serializable]
	public class ActivityUs 
	{
		/**活动id(新增时无需提供) */
		[XmlElement("id")]
			public int?  Id{ get; set; }

		/**活动名称 */
		[XmlElement("activityName")]
			public string  ActivityName{ get; set; }

		/**开始时间(YYYY-MM-DD) */
		[XmlElement("beginDate")]
			public string  BeginDate{ get; set; }

		/**结束时间(YYYY-MM-DD) */
		[XmlElement("endDate")]
			public string  EndDate{ get; set; }

		/**活动状态(1：待持券商家审核，2：持券商家审核未通过， 3：待运营人员审核，4：审核不通过，5：审核通过，6，中止) */
		[XmlElement("activityState")]
			public int?  ActivityState{ get; set; }

		/**活动类型(1：全体用户，2:指定类目,3:指定商品) */
		[XmlElement("activityType")]
			public int?  ActivityType{ get; set; }

		/**活动发起商家id */
		[XmlElement("merchantId")]
			public long?  MerchantId{ get; set; }

		/**活动发起商家名称 */
		[XmlElement("merchantName")]
			public string  MerchantName{ get; set; }

		/**持券商家id */
		[XmlElement("referMerchantId")]
			public long?  ReferMerchantId{ get; set; }

		/**持券商家名称(商家店铺名称) */
		[XmlElement("referMerchantName")]
			public string  ReferMerchantName{ get; set; }

		/**申请抵用券关联的活动id */
		[XmlElement("couponActivityId")]
			public int?  CouponActivityId{ get; set; }

		/**抵用券关联活动名称 */
		[XmlElement("couponActivityName")]
			public string  CouponActivityName{ get; set; }

		/**申请的抵用券数量 */
		[XmlElement("couponNum")]
			public int?  CouponNum{ get; set; }

		/**抵用券剩余量 */
		[XmlElement("couponLeftNum")]
			public int?  CouponLeftNum{ get; set; }

		/**活动创建时间 */
		[XmlElement("createTime")]
			public string  CreateTime{ get; set; }

		/**活动申请商家更新时间 */
		[XmlElement("updateTime")]
			public string  UpdateTime{ get; set; }

		/**持券商家审核时间 */
		[XmlElement("merchantVerifyTime")]
			public string  MerchantVerifyTime{ get; set; }

		/**后台审核时间 */
		[XmlElement("backendVerifyTime")]
			public string  BackendVerifyTime{ get; set; }

		/**运营审核人id */
		[XmlElement("backendVerifyId")]
			public long?  BackendVerifyId{ get; set; }

		/**运营审核人名称 */
		[XmlElement("backendVerifyName")]
			public string  BackendVerifyName{ get; set; }

		/**备注(申请商家填写的备注信息) */
		[XmlElement("activityRemark")]
			public string  ActivityRemark{ get; set; }

		/**备注（审核不通过及中止时) */
		[XmlElement("verifyRemark")]
			public string  VerifyRemark{ get; set; }

		/**活动关联的类目信息列表 */
		[XmlElement("categoryList")]
		public ActivityUsCategoryList  CategoryList{ get; set; }

		/**活动关联的商品列表 */
		[XmlElement("productList")]
		public ActivityUsProductList  ProductList{ get; set; }

	}
}
