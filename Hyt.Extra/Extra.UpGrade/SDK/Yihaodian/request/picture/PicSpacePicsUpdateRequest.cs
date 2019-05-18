using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量更新图片空间图片信息
	/// </summary>
	public class PicSpacePicsUpdateRequest 
		: IYhdRequest<PicSpacePicsUpdateResponse> 
	{
		/**图片空间图片更新信息 由picSpaceId:picName:picCategoryId组成，不同图片信息逗号分隔。 picSpaceId:图片在图片空间的唯一标识； picName:新的图片名称； picCategoryId:新的类别Id。 */
			public string  PicInfoList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.pic.space.pics.update";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("picInfoList", this.PicInfoList);
			return parameters;
		}
		#endregion
	}
}
