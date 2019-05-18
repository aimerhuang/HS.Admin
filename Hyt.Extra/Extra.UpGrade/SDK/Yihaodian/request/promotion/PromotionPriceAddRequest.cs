using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 新增价格促销
	/// </summary>
	public class PromotionPriceAddRequest 
		: IYhdRequest<PromotionPriceAddResponse> 
	{
		/**促销开始时间，产品促销时间不能超过一个月！格式：yyyy-MM-dd HH:mm:ss */
			public string  StartDate{ get; set; }

		/**促销结束时间，产品促销时间不能超过一个月！格式：yyyy-MM-dd HH:mm:ss */
			public string  EndDate{ get; set; }

		/**产品串="产品id,折扣/促销价,单用户购买数量限制,总数量限制" 产品可以是普通产品和子品，不能是系列产品。0为不限制。isPrice为0时存折扣值，为1时存促销价格值 */
			public string  ProductStr{ get; set; }

		/**系列虚品产品串中的产品只能为系列虚品，子产品串中的产品只能为该系列虚品的子品。 如果系列虚品产品串后一个子产品串也没有则添加所有子品促销。 虚品后有子产品串则虚品的价格值必须为0,其系列产品的新增格式为 系列产品id,0,单用户购买数量限制,总数量限制:系列子产品id1,折扣/促销价,单用户购买数量限制,总数量限制:系列子产品id2,折扣/促销价,单用户购买数量限制,总数量限制 */
			public string  ProductStrSerial{ get; set; }

		/**是否附带设置免邮促销，1：是  0：否 */
			public int?  IsAttachShippingProm{ get; set; }

		/**单独设置的邮费（该邮费只有在全场免邮的情况下才会生效），附带设置免邮时为0。isAttachShippingProm和postage同时为0时使用默认邮费 */
			public int?  Postage{ get; set; }

		/**0：按折扣设置，1：按价格设置 */
			public int?  IsPrice{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.promotion.price.add";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("startDate", this.StartDate);
			parameters.Add("endDate", this.EndDate);
			parameters.Add("productStr", this.ProductStr);
			parameters.Add("productStrSerial", this.ProductStrSerial);
			parameters.Add("isAttachShippingProm", this.IsAttachShippingProm);
			parameters.Add("postage", this.Postage);
			parameters.Add("isPrice", this.IsPrice);
			return parameters;
		}
		#endregion
	}
}
