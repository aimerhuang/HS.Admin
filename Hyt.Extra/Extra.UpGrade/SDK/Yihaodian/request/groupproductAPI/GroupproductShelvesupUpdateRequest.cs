using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 上架集团商家产品
	/// </summary>
	public class GroupproductShelvesupUpdateRequest 
		: IYhdRequest<GroupproductShelvesupUpdateResponse> 
	{
		/**1号店产品ID,与outerId二选一 */
			public long?  ProductId{ get; set; }

		/**外部产品ID,与productId二选一 */
			public string  OuterId{ get; set; }

		/**区域商家ID（多个以逗号分隔） */
			public string  AreaMerchantIds{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.groupproduct.shelvesup.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("areaMerchantIds", this.AreaMerchantIds);
			return parameters;
		}
		#endregion
	}
}
