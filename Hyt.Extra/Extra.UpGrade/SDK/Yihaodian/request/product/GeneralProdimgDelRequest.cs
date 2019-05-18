using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 删除单品图片
	/// </summary>
	public class GeneralProdimgDelRequest 
		: IYhdRequest<GeneralProdimgDelResponse> 
	{
		/**1号店产品ID,与outerId二选一(productId优先) */
			public long?  ProductId{ get; set; }

		/**外部产品ID,与productId二选一 */
			public string  OuterId{ get; set; }

		/**要删除的图片id列表（不同图片id用逗号分隔），pids为空，表示删除此产品下所有图片 */
			public string  Pids{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.general.prodimg.del";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("pids", this.Pids);
			return parameters;
		}
		#endregion
	}
}
