using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 新增图片空间类别
	/// </summary>
	public class PicSpaceCategorysAddRequest 
		: IYhdRequest<PicSpaceCategorysAddResponse> 
	{
		/**图片类别名称 */
			public string  PicCategoryName{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.pic.space.categorys.add";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("picCategoryName", this.PicCategoryName);
			return parameters;
		}
		#endregion
	}
}
