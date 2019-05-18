using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询类别基本属性(不包含系列属性)
	/// </summary>
	public class CategoryAttributeGetRequest 
		: IYhdRequest<CategoryAttributeGetResponse> 
	{
		/**类别ID(二选一，categoryId优先，备注：必须为叶子类目ID) */
			public long?  CategoryId{ get; set; }

		/**属性ID(二选一) */
			public long?  AttributeId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.category.attribute.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("categoryId", this.CategoryId);
			parameters.Add("attributeId", this.AttributeId);
			return parameters;
		}
		#endregion
	}
}
