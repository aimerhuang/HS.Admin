using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 系列子品文描更新（文描包括文描信息，包装清单和售后服务）
	/// </summary>
	public class SerialDescriptionUpdateRequest 
		: IYhdRequest<SerialDescriptionUpdateResponse> 
	{
		/**1号店产品ID,与outerId二选一(productId优先)，productId是系列子品id */
			public long?  ProductId{ get; set; }

		/**外部产品ID,与productId二选一 */
			public string  OuterId{ get; set; }

		/**系列子品文描信息 */
			public string  ProductDescription{ get; set; }

		/**系列子品包装清单 */
			public string  PackingList{ get; set; }

		/**系列子品售后服务 */
			public string  AfterSaleService{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.serial.description.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("productDescription", this.ProductDescription);
			parameters.Add("packingList", this.PackingList);
			parameters.Add("afterSaleService", this.AfterSaleService);
			return parameters;
		}
		#endregion
	}
}
