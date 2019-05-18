using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量查询图片空间分类信息
	/// </summary>
	public class PicSpaceCategorysGetRequest 
		: IYhdRequest<PicSpaceCategorysGetResponse> 
	{
		/**图片分类ID列表。多个分类ID之间用逗号分隔，最大100个。 */
			public string  PicCategoryIdList{ get; set; }

		/**图片空间分类名称。图片空间分类名称。支持模糊查询。 */
			public string  PicCategoryName{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.pic.space.categorys.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("picCategoryIdList", this.PicCategoryIdList);
			parameters.Add("picCategoryName", this.PicCategoryName);
			return parameters;
		}
		#endregion
	}
}
