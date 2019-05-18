using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取单个系列产品的子品信息
	/// </summary>
	public class SerialProductGetRequest 
		: IYhdRequest<SerialProductGetResponse> 
	{
		/**1号店产品ID(系列产品id,与productCode,outerId三选一,优先于outerId,productCode) */
			public long?  ProductId{ get; set; }

		/**外部产品编码(系列产品outerId,与productId, productCode三选一, 优于productCode) */
			public string  OuterId{ get; set; }

		/**品牌Id */
			public long?  BrandId{ get; set; }

		/**系列产品编码(与productId, outerId三选一) */
			public string  ProductCode{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.serial.product.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			parameters.Add("brandId", this.BrandId);
			parameters.Add("productCode", this.ProductCode);
			return parameters;
		}
		#endregion
	}
}
