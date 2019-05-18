using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量上传图片到图片空间
	/// </summary>
	public class PicSpacePicsUploadRequest 
		: IYhdRequest<PicSpacePicsUploadResponse> 
	{
		/**图片分类ID(默认为默认分类) */
			public long?  PicCategoryId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.pic.space.pics.upload";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("picCategoryId", this.PicCategoryId);
			return parameters;
		}
		#endregion
	}
}
