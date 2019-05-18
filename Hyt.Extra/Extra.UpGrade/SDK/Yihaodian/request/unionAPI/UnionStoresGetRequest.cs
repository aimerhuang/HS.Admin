using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.Yihaodian.Response;


namespace Extra.UpGrade.SDK.Yihaodian.Request
{
	/// <summary>
	/// 批量查询网盟店铺信息
	/// </summary>
	public class UnionStoresGetRequest 
		: IYhdRequest<UnionStoresGetResponse> 
	{
		/**联盟用户ID */
			public long?  TrackerU{ get; set; }

		/**网站ID */
			public string  WebsiteId{ get; set; }

		/**用户ID */
			public long?  Uid{ get; set; }

		/**商家ID */
			public long?  UserId{ get; set; }

		/**商家所在地省份名称 */
			public string  MerchantProvinceName{ get; set; }

		/**近期支出佣金量，开始值 */
			public int?  RecentlyCommissionExpensesStart{ get; set; }

		/**近期支出佣金量，结束值 */
			public int?  RecentlyCommissionExpensesEnd{ get; set; }

		/**指明返回哪些字段数据,用英文逗号分隔。根据请求数据字段，返回相应字段数据。默认不填为返回全部字段(可选项:user_id,seller_nick,shop_title,pic_url,shop_url,commission_rate,auction_count,merchant_province_name,item_score,item_status,item_differ,service_score,service_status,service_differ,delivery_score,delivery_status,delivery_differ,commision_level,merchant_categorys) */
			public string  Fields{ get; set; }

		/**商家店铺名称关键字 */
			public string  Keyword{ get; set; }

		/**商家经营类目ID，暂时保留 */
			public long?  Cid{ get; set; }

		/**商家佣金比例查询开始值 */
			public string  StartCommissionrate{ get; set; }

		/**商家佣金比例查询结束值 */
			public string  EndCommissionrate{ get; set; }

		/**商家宝贝数量查询开始值 */
			public string  StartAuctioncount{ get; set; }

		/**商家宝贝数量查询结束值 */
			public string  EndAuctioncount{ get; set; }

		/**排序字段 目前支持的排序字段有("commision_rate","auction_count")两个属性，否则排序无效，其它内容时为错误信息 */
			public string  SortField{ get; set; }

		/**排序类型，必须是desc和asc其中之一。不填时默认按照asc处理 */
			public string  SortType{ get; set; }

		/**当前页码 */
			public int?  PageNo{ get; set; }

		/**每页最大条数(默认40，最大是40条，超过40时用最大值代替) */
			public int?  PageSize{ get; set; }

		/**店铺的信用等级总共为20级 1-5:1heart-5heart;6-10:1diamond-5diamond;11-15:1crown-5crown;16-20:1goldencrown-5goldencrown,暂时保留 */
			public string  StartCredit{ get; set; }

		/**店铺的信用等级总共为20级 1-5:1heart-5heart;6-10:1diamond-5diamond;11-15:1crown-5crown;16-20:1goldencrown-5goldencrown，暂时保留 */
			public string  EndCredit{ get; set; }

		/**店铺累计推广量开始值,暂时保留 */
			public string  StartTotalaction{ get; set; }

		/**店铺累计推广数查询结束值，暂时保留 */
			public string  EndTotalaction{ get; set; }

		/**是否只显示商城店铺,暂时保留 */
			public bool  OnlyMall{ get; set; }

		/**标识一个应用是否来在无线或者手机应用,如果是true则会使用其他规则加密点击串.如果不传值,则默认是false,暂时保留 */
			public bool  IsMobile{ get; set; }


		#region IYhdRequest Members
	
		public string GetApiName()
		{
			return "yhd.union.stores.get";
		}
		
		public IDictionary<string, string> GetParameters()
		{
			YhdDictionary parameters = new YhdDictionary();
			parameters.Add("trackerU", this.TrackerU);
			parameters.Add("websiteId", this.WebsiteId);
			parameters.Add("uid", this.Uid);
			parameters.Add("userId", this.UserId);
			parameters.Add("merchantProvinceName", this.MerchantProvinceName);
			parameters.Add("recentlyCommissionExpensesStart", this.RecentlyCommissionExpensesStart);
			parameters.Add("recentlyCommissionExpensesEnd", this.RecentlyCommissionExpensesEnd);
			parameters.Add("fields", this.Fields);
			parameters.Add("keyword", this.Keyword);
			parameters.Add("cid", this.Cid);
			parameters.Add("startCommissionrate", this.StartCommissionrate);
			parameters.Add("endCommissionrate", this.EndCommissionrate);
			parameters.Add("startAuctioncount", this.StartAuctioncount);
			parameters.Add("endAuctioncount", this.EndAuctioncount);
			parameters.Add("sortField", this.SortField);
			parameters.Add("sortType", this.SortType);
			parameters.Add("pageNo", this.PageNo);
			parameters.Add("pageSize", this.PageSize);
			parameters.Add("startCredit", this.StartCredit);
			parameters.Add("endCredit", this.EndCredit);
			parameters.Add("startTotalaction", this.StartTotalaction);
			parameters.Add("endTotalaction", this.EndTotalaction);
			parameters.Add("onlyMall", this.OnlyMall);
			parameters.Add("isMobile", this.IsMobile);
			return parameters;
		}
		#endregion
	}
}
