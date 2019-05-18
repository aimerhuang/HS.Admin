using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量查询系列子品信息
	/// </summary>
	public class SerialChildproductsGetRequest 
		: IYhdRequest<SerialChildproductsGetResponse> 
	{
		/**1号店产品ID列表（逗号分隔）与outerIdList、productCodeList三选一,最大长度为100，优先级最高 */
			public string  ProductIdList{ get; set; }

		/**外部产品ID列表（逗号分隔）与productIdList、productCodeList三选一,最大长度为100，每个最多30字符，优先级次之 */
			public string  OuterIdList{ get; set; }

		/**产品编码列表（逗号分隔）与productIdList、outerIdList三选一,最大长度为100,每个最多30字符，优先级最低 */
			public string  ProductCodeList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.serial.childproducts.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("productIdList", this.ProductIdList);
			parameters.Add("outerIdList", this.OuterIdList);
			parameters.Add("productCodeList", this.ProductCodeList);
			return parameters;
		}
		#endregion
	}
}
