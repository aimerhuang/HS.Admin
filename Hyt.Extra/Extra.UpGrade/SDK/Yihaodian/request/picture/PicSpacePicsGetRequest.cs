using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 查询图片空间图片信息
	/// </summary>
	public class PicSpacePicsGetRequest 
		: IYhdRequest<PicSpacePicsGetResponse> 
	{
		/**图片空间类别ID  */
			public long?  PicCategoryId{ get; set; }

		/**图片名称 */
			public string  PicName{ get; set; }

		/**当前页数 */
			public int?  CurPage{ get; set; }

		/**每页显示记录数 */
			public int?  PageRows{ get; set; }

		/**图片空间id(用于图片空间图片的新增，删除，修改，查询功能) */
			public string  PicSpaceIdList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.pic.space.pics.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("picCategoryId", this.PicCategoryId);
			parameters.Add("picName", this.PicName);
			parameters.Add("curPage", this.CurPage);
			parameters.Add("pageRows", this.PageRows);
			parameters.Add("picSpaceIdList", this.PicSpaceIdList);
			return parameters;
		}
		#endregion
	}
}
