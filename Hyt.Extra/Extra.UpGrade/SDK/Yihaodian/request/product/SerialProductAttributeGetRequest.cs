using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 获取商品系列属性
	/// </summary>
	public class SerialProductAttributeGetRequest 
		: IYhdRequest<SerialProductAttributeGetResponse> 
	{
		/**一号店（虚品或子品的）产品ID，与outerId二选一 */
			public long?  ProductId{ get; set; }

		/**（虚品或子品的）外部产品ID，与productId二选一 */
			public string  OuterId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.serial.product.attribute.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productId", this.ProductId);
			parameters.Add("outerId", this.OuterId);
			return parameters;
		}
		#endregion
	}
}
