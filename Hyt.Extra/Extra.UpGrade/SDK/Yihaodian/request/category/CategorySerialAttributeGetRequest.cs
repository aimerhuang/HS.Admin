using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询类别系列属性
	/// </summary>
	public class CategorySerialAttributeGetRequest 
		: IYhdRequest<CategorySerialAttributeGetResponse> 
	{
		/**类别ID */
			public long?  CategoryId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.category.serial.attribute.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("categoryId", this.CategoryId);
			return parameters;
		}
		#endregion
	}
}
