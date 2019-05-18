using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 团购报名查询
	/// </summary>
	public class GroupBuyProductsGetRequest 
		: IYhdRequest<GroupBuyProductsGetResponse> 
	{
		/**当前页数 */
			public int?  CurPage{ get; set; }

		/**每页显示记录数，默认50，最大100 */
			public int?  PageRows{ get; set; }

		/**团购开始时间查询起始时间(yyyy-MM-ddHH:mm:ss格式) */
			public string  StartTimeBegin{ get; set; }

		/**团购开始时间查询结束时间(yyyy-MM-ddHH:mm:ss格式) */
			public string  StartTimeEnd{ get; set; }

		/**团购结束时间查询起始时间(yyyy-MM-ddHH:mm:ss格式) */
			public string  EndTimeBegin{ get; set; }

		/**团购结束时间查询截止时间(yyyy-MM-ddHH:mm:ss格式) */
			public string  EndTimeEnd{ get; set; }

		/**团购状态(WAIT_VERIFYING:待审核;VERIFY_REFUESD一审拒绝;VERIFY_REFUESD_2:二审拒绝;VERIFY_BACK:一审打回;VERIFY_BACK_2:二审打回;VERIFY_SUPPLEMENT:一审待补充;QUALITY_PAY:待质检打款;VERIFY_PASSED_1:一审通过;VERIFY_PASSED:审核通过;GROUPON:团购中;GROUPON_SUCCESS:团购中-成功;GROUPON_FULLED:团购中-人数满;FINISHED_FAILUE:结束-失败;FINISHED_SUCCESS:结束-成功;GROUP_FINISHED:处理完毕) */
			public string  GroupStatus{ get; set; }

		/**团购id列表 */
			public string  GroupIdList{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.group.buy.products.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("curPage", this.CurPage);
			parameters.Add("pageRows", this.PageRows);
			parameters.Add("startTimeBegin", this.StartTimeBegin);
			parameters.Add("startTimeEnd", this.StartTimeEnd);
			parameters.Add("endTimeBegin", this.EndTimeBegin);
			parameters.Add("endTimeEnd", this.EndTimeEnd);
			parameters.Add("groupStatus", this.GroupStatus);
			parameters.Add("groupIdList", this.GroupIdList);
			return parameters;
		}
		#endregion
	}
}
