using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 新增系列产品子品
	/// </summary>
	public class SerialChildProductsAddRequest 
		: IYhdRequest<SerialChildProductsAddResponse> 
	{
		/**系列产品外部编码 */
			public string  OuterId{ get; set; }

		/**系列子产品新增信息<br/>(outerId:isMainProduct:colorItemName:colorItemId:sizeItemId:productMarketPrice:productSalePrice:virtualStockNum:subsidyAmount:properties)<br/> <font color="red"> <b>注意：</b><br/>1. properties是新增字段，用来代替colorItemName、colorItemId和sizeItemId字段。若添加了properties字段，将忽略前面的colorItemName、colorItemId和sizeItemId字段的值。<br/>2. 由于国家停止了节能补贴政策，subsidyAmount字段暂时保留。拼接字符串时，占一位。（拼接时，填0即可）<br/>3. 例子 outerId1:1:红色:5001:5010:200:180:5000:0:1001_5001_红色;1002_5010_XXL加大码 </font> */
			public string  SerialChildProductsList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.serial.childProducts.add";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("outerId", this.OuterId);
			parameters.Add("serialChildProductsList", this.SerialChildProductsList);
			return parameters;
		}
		#endregion
	}
}
