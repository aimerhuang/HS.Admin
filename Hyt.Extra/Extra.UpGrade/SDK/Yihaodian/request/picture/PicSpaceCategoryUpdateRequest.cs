using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 更新图片空间类别
	/// </summary>
	public class PicSpaceCategoryUpdateRequest 
		: IYhdRequest<PicSpaceCategoryUpdateResponse> 
	{
		/**图片空间类别Id */
			public long?  PicCategoryId{ get; set; }

		/**图片空间类别名称（最长10个字） */
			public string  PicCategoryName{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.pic.space.category.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("picCategoryId", this.PicCategoryId);
			parameters.Add("picCategoryName", this.PicCategoryName);
			return parameters;
		}
		#endregion
	}
}
