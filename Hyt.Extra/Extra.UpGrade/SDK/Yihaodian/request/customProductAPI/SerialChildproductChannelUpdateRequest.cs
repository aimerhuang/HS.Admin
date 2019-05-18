using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 指定子品文描为虚品文描或者子品文描
	/// </summary>
	public class SerialChildproductChannelUpdateRequest 
		: IYhdRequest<SerialChildproductChannelUpdateResponse> 
	{
		/**1号店虚品产品ID,与outerId二选一(productId优先)，productId为虚品id */
			public long?  ProductId{ get; set; }

		/**外部产品ID,与productId二选一 */
			public string  OuterId{ get; set; }

		/**是否读取系列子品文描：1表示是，0表示否 */
			public long?  IsReadSubDesc{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.serial.childproduct.channel.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("isReadSubDesc", this.IsReadSubDesc);
			return parameters;
		}
		#endregion
	}
}
