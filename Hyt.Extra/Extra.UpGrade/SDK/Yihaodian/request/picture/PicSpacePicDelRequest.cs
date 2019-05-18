using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 删除图片空间的图片
	/// </summary>
	public class PicSpacePicDelRequest 
		: IYhdRequest<PicSpacePicDelResponse> 
	{
		/**图片空间的图片唯一标识 */
			public long?  PicSpaceId{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.pic.space.pic.del";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("picSpaceId", this.PicSpaceId);
			return parameters;
		}
		#endregion
	}
}
