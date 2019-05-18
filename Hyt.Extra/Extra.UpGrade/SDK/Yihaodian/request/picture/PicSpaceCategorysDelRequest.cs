using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量删除图片空间分类
	/// </summary>
	public class PicSpaceCategorysDelRequest 
		: IYhdRequest<PicSpaceCategorysDelResponse> 
	{
		/**图片分类ID列表。多个分类ID之间用逗号分隔，最大100个。 */
			public string  PicCategoryIdList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.pic.space.categorys.del";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("picCategoryIdList", this.PicCategoryIdList);
			return parameters;
		}
		#endregion
	}
}
