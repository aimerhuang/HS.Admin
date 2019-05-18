using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 更新集团商家单个产品库存信息
	/// </summary>
	public class GroupproductStockUpdateRequest 
		: IYhdRequest<GroupproductStockUpdateResponse> 
	{
		/**1号店产品ID,与outerId二选一.存在非法字符默认为空(productId优先) */
			public long?  ProductId{ get; set; }

		/**外部产品ID,与productId二选一 */
			public string  OuterId{ get; set; }

		/**传参格式（区域商家id：仓库id:库存数;）,字段间用英文冒号分隔，多个对象用英文分号分隔。仓库id可以不传。 */
			public string  MerchantIdWarehouseIdStockNumList{ get; set; }

		/**更新类型，默认为1（1：全量更新，2：增量更新） */
			public int?  UpdateType{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.groupproduct.stock.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("merchantIdWarehouseIdStockNumList", this.MerchantIdWarehouseIdStockNumList);
			parameters.Add("updateType", this.UpdateType);
			return parameters;
		}
		#endregion
	}
}
